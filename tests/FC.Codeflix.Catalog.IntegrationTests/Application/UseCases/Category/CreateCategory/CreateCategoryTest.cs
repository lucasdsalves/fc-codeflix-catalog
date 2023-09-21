using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;
using CreateCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        public CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            // Arrange
            var dbContext = _fixture.CreateDbContext();

            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);

            var useCase = new CreateCategoryUseCase.CreateCategory(repository, unitOfWork);

            var input = _fixture.GetCategoryInput();

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
            (output.Id != Guid.Empty).Should().BeTrue();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
        }
    }
}
