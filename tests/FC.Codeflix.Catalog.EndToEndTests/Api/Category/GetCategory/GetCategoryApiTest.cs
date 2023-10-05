using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory
{
    [Collection(nameof(GetCategoryApiTestFixture))]

    public class GetCategoryApiTest
    {
        private readonly GetCategoryApiTestFixture _fixture;

        public GetCategoryApiTest(GetCategoryApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task GetCategory()
        {
            // Arrange
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await _fixture.Persistence.InsertList(sampleCategoryList);

            var sampleCategory = sampleCategoryList[2];

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Get<CategoryModelOutput>($"/categories/{sampleCategory.Id}");

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);

            output.Should().NotBeNull();
            output!.Id.Should().Be(sampleCategory.Id);
            output!.Name.Should().Be(sampleCategory.Name);
            output.Description.Should().Be(sampleCategory.Description);
            output.IsActive.Should().Be(sampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(ThrowWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task ThrowWhenNotFound()
        {
            // Arrange
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await _fixture.Persistence.InsertList(sampleCategoryList);
            var randomId = Guid.NewGuid();

            var sampleCategory = sampleCategoryList[2];

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Get<ProblemDetails>($"/categories/{randomId}");

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

            output.Should().NotBeNull();
            output!.Status.Should().Be(StatusCodes.Status404NotFound);
            output.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Detail.Should().Be($"Category '{randomId}' not found");
        }
    }
}
