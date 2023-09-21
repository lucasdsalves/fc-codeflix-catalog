using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;
using ListCategoriesUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;

        public ListCategoriesTest(ListCategoriesTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact(DisplayName = nameof(ListCategories))]
        [Trait("Integration/Application", "ListCategories - Use Cases")]
        public async Task ListCategories()
        {
            // Arrange
            var dbContext = _fixture.CreateDbContext();

            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync();

            var repository = new CategoryRepository(dbContext);
            var input = new ListCategoriesInput(1, 5, "", "", SearchOrder.Asc);

            var useCase = new ListCategoriesUseCase.ListCategories(repository);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            output.Page.Should().Be(input.Page);
            output.PerPage.Should().Be(input.PerPage);
            output.Total.Should().Be(sampleCategoryList.Count);
            output.Items.Should().HaveCount(sampleCategoryList.Count);
        }
    }
}
