using GetCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;
        private readonly Mock<ICategoryRepository> _repositoryMock;

        public GetCategoryTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<ICategoryRepository>();
        }

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Application", "GetCategory - Use Cases")]
        public async Task GetCategory()
        {
            // Arrange
            var sampleCategory = _fixture.GetValidCategory();

            _repositoryMock.Setup(x => x.Get(
                                        It.IsAny<Guid>(),
                                        It.IsAny<CancellationToken>()
                           )).ReturnsAsync(sampleCategory);

            var input = new GetCategoryUseCase.GetCategoryInput(sampleCategory.Id);

            var useCase = new GetCategoryUseCase.GetCategory(_repositoryMock.Object);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repository =>
                                 repository.Get(
                                                It.IsAny<Guid>(),
                                                It.IsAny<CancellationToken>()),
                                                Times.Once);

            output.Should().NotBeNull();
            (output.Id != Guid.Empty).Should().BeTrue();
            output.Name.Should().Be(sampleCategory.Name);
            output.Description.Should().Be(sampleCategory.Description);
            output.IsActive.Should().Be(sampleCategory.IsActive);
        }
    }
}
