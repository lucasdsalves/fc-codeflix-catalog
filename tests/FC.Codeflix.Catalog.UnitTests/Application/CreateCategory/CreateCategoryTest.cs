using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using CreateCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new Mock<ICategoryRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            // Arrange
            var useCase = new CreateCategoryUseCase.CreateCategory(_repositoryMock.Object, _unitOfWorkMock.Object);

            var input = _fixture.GetInput();

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repository =>
                                             repository.Insert(
                                                            It.IsAny<Category>(),
                                                            It.IsAny<CancellationToken>()),
                                                            Times.Once);

            _unitOfWorkMock.Verify(uow =>
                                        uow.Commit(
                                            It.IsAny<CancellationToken>()),
                                            Times.Once);

            output.Should().NotBeNull();
            (output.Id != Guid.Empty).Should().BeTrue();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
        [Trait("Application", "CreateCategory - Use Cases")]
        [MemberData(nameof(GetInvalidInputs))]
        public async void ThrowWhenCantInstantiateAggregate(CreateCategoryInput input, string exceptionMessage)
        {
            var useCase = new CreateCategoryUseCase.CreateCategory(_repositoryMock.Object, _unitOfWorkMock.Object);

            Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

            await task.Should()
                      .ThrowAsync<EntityValidationException>()
                      .WithMessage(exceptionMessage);
        }

        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateCategoryTestFixture();
            var invalidInputList = new List<object[]>();

            var invalidInputShortName = fixture.GetInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

            invalidInputList.Add(new object[]
            {
                invalidInputShortName,
                "Name should not be less than 3 characters long"
            });

            var invalidInputDescriptionNull = fixture.GetInput();
            invalidInputDescriptionNull.Description = null!;

            invalidInputList.Add(new object[]
            {
                invalidInputDescriptionNull,
                "Description should not be null or empty"
            });

            return invalidInputList;
        }
    }
}
