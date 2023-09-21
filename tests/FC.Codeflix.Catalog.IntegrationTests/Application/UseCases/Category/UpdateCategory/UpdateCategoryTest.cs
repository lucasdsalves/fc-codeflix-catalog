using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;
using UpdateCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        public async Task UpdateCategory()
        {
            // Arrange
            var sampleCategory = _fixture.GetSampleCategory();
            
            var dbContext = _fixture.CreateDbContext();
            var trackingInfo = await dbContext.AddAsync(sampleCategory);
            await dbContext.SaveChangesAsync();
            trackingInfo.State = EntityState.Detached;

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new UpdateCategoryUseCase.UpdateCategory(repository, unitOfWork);

            var input = _fixture.GetCategoryInput(sampleCategory.Id);
            input.Name = sampleCategory.Name;
            input.Description = sampleCategory.Description;
            
            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            var dbCategory = await (_fixture.CreateDbContext(true))
                                .Categories.FindAsync(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
        }
    }
}
