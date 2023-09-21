using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;
using GetCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Integration/Application", "GetCategory - Use Cases")]
        public async Task GetCategory()
        {
            // Arrange
            var sampleCategory = _fixture.GetSampleCategory();

            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.Add(sampleCategory);
            dbContext.SaveChanges();

            var repository = new CategoryRepository(dbContext);

            var input = new GetCategoryUseCase.GetCategoryInput(sampleCategory.Id);

            var useCase = new GetCategoryUseCase.GetCategory(repository);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            (output.Id != Guid.Empty).Should().BeTrue();
            output.Name.Should().Be(sampleCategory.Name);
            output.Description.Should().Be(sampleCategory.Description);
            output.IsActive.Should().Be(sampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Integration/Application", "GetCategory - Use Cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            // Arrange
            var sampleCategory = _fixture.GetSampleCategory();

            var dbContext = _fixture.CreateDbContext();
            dbContext.Categories.Add(sampleCategory);
            dbContext.SaveChanges();

            var repository = new CategoryRepository(dbContext);

            var input = new GetCategoryUseCase.GetCategoryInput(Guid.NewGuid());

            var useCase = new GetCategoryUseCase.GetCategory(repository);

            // Act
            var task = async () => await useCase.Handle(input, CancellationToken.None);

            // Assert
            await task.Should().ThrowAsync<NotFoundException>()
                 .WithMessage($"Category '{input.Id}' not found");
        }
    }
}
