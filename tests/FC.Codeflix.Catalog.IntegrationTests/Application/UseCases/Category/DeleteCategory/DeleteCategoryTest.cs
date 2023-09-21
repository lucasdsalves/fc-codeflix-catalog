using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;
using DeleteCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Integration/Application", "DeleteCategory - Use Cases")]
        public async void DeleteCategory()
        {
            // Arrange
            var dbContext = _fixture.CreateDbContext();

            var sampleCategory = _fixture.GetSampleCategory();
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            
            await dbContext.AddRangeAsync(sampleCategoryList);
            var trackingInfo = await dbContext.AddAsync(sampleCategory);
            
            await dbContext.SaveChangesAsync();
            trackingInfo.State = EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new DeleteCategoryUseCase.DeleteCategory(repository, unitOfWork);

            var input = new DeleteCategoryInput(sampleCategory.Id);

            // Act
            await useCase.Handle(input, CancellationToken.None);

            // aSSERT
            var assertDbContext = _fixture.CreateDbContext(true);
            var dbCategoryDeleted = await assertDbContext.Categories
                                                         .FindAsync(sampleCategory.Id);
            dbCategoryDeleted.Should().BeNull();
            var dbCategories = await assertDbContext.Categories.ToListAsync();
            dbCategories.Should().HaveCount(sampleCategoryList.Count);
        }
    }
}
