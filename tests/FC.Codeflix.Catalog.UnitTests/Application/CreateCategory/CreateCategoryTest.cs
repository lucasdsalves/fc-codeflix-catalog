using CreateCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    public class CreateCategoryTest
    {
        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            // Arrange
            var repositoryMock = new Mock<ICategoryRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var useCase = new CreateCategoryUseCase.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var input = new CreateCategoryUseCase.CreateCategoryInput(
                "Category Name",
                "Category Description",
                true);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            repositoryMock.Verify(repository => 
                                             repository.Insert(
                                                            It.IsAny<Category>(),
                                                            It.IsAny<CancellationToken>()), 
                                                            Times.Once);

            unitOfWorkMock.Verify(uow => 
                                        uow.Commit(
                                            It.IsAny<CancellationToken>()),
                                            Times.Once);

            output.Should().NotBeNull();
            (output.Id != Guid.Empty).Should().BeTrue();
            output.Name.Should().Be("Category Name");
            output.Description.Should().Be("Category Description");
            output.IsActive.Should().BeTrue();  
        }
    }
}
