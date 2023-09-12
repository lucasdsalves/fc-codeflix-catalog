using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using UpdateCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryDataGenerator.GetCategoriesToUpdate), parameters: 5,
                    MemberType = typeof(UpdateCategoryDataGenerator))]
        public async void UpdateCategory(Category sampleCategory, UpdateCategoryInput input)
        {
            // Arrange
            _repositoryMock.Setup(x => x.Get(
                            sampleCategory.Id,
                            It.IsAny<CancellationToken>()
               )).ReturnsAsync(sampleCategory);

             // Act
            var useCase = new UpdateCategoryUseCase.UpdateCategory(_repositoryMock.Object, _unitOfWorkMock.Object);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(sampleCategory.Name);
            output.Description.Should().Be(sampleCategory.Description);
            output.IsActive.Should().Be(input.IsActive);

            _repositoryMock.Verify(repository =>
                     repository.Get(
                                    sampleCategory.Id,
                                    It.IsAny<CancellationToken>()),
                                    Times.Once);

            _repositoryMock.Verify(repository =>
                    repository.Update(
                                    sampleCategory,
                                    It.IsAny<CancellationToken>()),
                                    Times.Once);

            _unitOfWorkMock.Verify(uof =>
                    uof.Commit(
                        It.IsAny<CancellationToken>()),
                        Times.Once);
        }

        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        public async void ThrowWhenCategoryNotFound()
        {
            // Arrange
            var input = _fixture.GetValidInput();

            _repositoryMock.Setup(x => x.Get(
                            input.Id,
                            It.IsAny<CancellationToken>()
               )).ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));

            // Act
            var useCase = new UpdateCategoryUseCase.UpdateCategory(_repositoryMock.Object, _unitOfWorkMock.Object);

            var task = async() => await useCase.Handle(input, CancellationToken.None);

            // Assert
            await task.Should().ThrowAsync<NotFoundException>();

            _repositoryMock.Verify(repository =>
                     repository.Get(
                                    input.Id,
                                    It.IsAny<CancellationToken>()),
                                    Times.Once);

        }
    }
}
