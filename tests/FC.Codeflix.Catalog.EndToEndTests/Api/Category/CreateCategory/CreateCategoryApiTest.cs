using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTest
    {
        private readonly CreateCategoryApiTestFixture _fixture;

        public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("EndToEnd/API", "Category - Endpoints")]
        public async Task CreateCategory()
        {
            // Arrange
            var input = _fixture.GetSampleInput();

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Post<CategoryModelOutput>("/categories", input);

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            output.Should().NotBeNull();
            output!.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);    
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();

            var dbCategory = await _fixture.Persistence.GetById(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
            dbCategory.Id.Should().NotBeEmpty();
        }
    }
}
