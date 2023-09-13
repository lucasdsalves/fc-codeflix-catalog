using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using ListCategoriesUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public ListCategoriesTest(ListCategoriesTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact(DisplayName = nameof(ListCategories))]
        [Trait("Application", "ListCategories - Use Cases")]
        public async Task ListCategories()
        {
            // Arrange
            var categoriesList = _fixture.GetSampleCategoryList();

            var input = new ListCategoriesUseCase.ListCategoriesInput(
                                page: 2,
                                perPage: 15,
                                search: "search-example",
                                sort: "name",
                                dir: SearchOrder.Asc
                            );

            var outputRepositorySearch = new OutputSearch<Category>(
                               currentPage: input.Page,
                               perPage: input.PerPage,
                               items: (IReadOnlyList<Category>)categoriesList,
                               total: 70);

            _repositoryMock.Setup(x => x.Search(
                    It.Is<SearchInput>(
                            searchInput =>
                                searchInput.Page == input.Page
                                && searchInput.PerPage == input.PerPage
                                && searchInput.Search == input.Search
                                && searchInput.OrderBy == input.Sort
                                && searchInput.Order == input.Dir
                            ),
                    It.IsAny<CancellationToken>()
                           )).ReturnsAsync(outputRepositorySearch);

            var useCase = new ListCategoriesUseCase.ListCategories(_repositoryMock.Object);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            output.Page.Should().Be(outputRepositorySearch.CurrentPage);
            output.PerPage.Should().Be(outputRepositorySearch.PerPage);
            output.Total.Should().Be(outputRepositorySearch.Total);
            output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);

            _repositoryMock.Verify(x => x.Search(
                    It.Is<SearchInput>(
                            searchInput =>
                                searchInput.Page == input.Page
                                && searchInput.PerPage == input.PerPage
                                && searchInput.Search == input.Search
                                && searchInput.OrderBy == input.Sort
                                && searchInput.Order == input.Dir
                            ),
                    It.IsAny<CancellationToken>()
                           ), Times.Once);
        }
    }
}
