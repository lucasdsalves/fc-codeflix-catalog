using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using DeleteCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async void DeleteCategory()
        {
            // Arrange
            var categorySample = _fixture.GetValidCategory();

            _repositoryMock.Setup(repository => repository.Get(categorySample.Id,
                                                               It.IsAny<CancellationToken>()))
                                                          .ReturnsAsync(categorySample);

            var input = new DeleteCategoryInput(categorySample.Id);

            var useCase = new DeleteCategoryUseCase.DeleteCategory(_repositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            await useCase.Handle(input, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(x => x.Get(categorySample.Id, 
                                              It.IsAny<CancellationToken>()) 
                                          ,Times.Once());

            _repositoryMock.Verify(x => x.Delete(categorySample, 
                                                 It.IsAny<CancellationToken>())
                                          ,Times.Once());

            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>())
                                         , Times.Once());
        }

        [Fact(DisplayName = nameof(ThrownWhenCategoryNotFound))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async void ThrownWhenCategoryNotFound()
        {
            // Arrange
            var sampleGuid = Guid.NewGuid();

            _repositoryMock.Setup(repository => repository.Get(sampleGuid,
                                                               It.IsAny<CancellationToken>()))
                                                          .ThrowsAsync(new NotFoundException($"Category '{sampleGuid} not found"));

            var input = new DeleteCategoryInput(sampleGuid);

            var useCase = new DeleteCategoryUseCase.DeleteCategory(_repositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            var task = async () => await useCase.Handle(input, CancellationToken.None);

            // Assert
            await task.Should().ThrowAsync<NotFoundException>();    

            _repositoryMock.Verify(x => x.Get(sampleGuid,
                                              It.IsAny<CancellationToken>())
                                          , Times.Once());
        }
    }
}
