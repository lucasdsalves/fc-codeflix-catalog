using FC.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryApiTestFixture))]

    public class UpdateCategoryApiTest
    {
        private readonly UpdateCategoryApiTestFixture _fixture;

        public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async Task UpdateCategory()
        {
            // Arrange
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await _fixture.Persistence.InsertList(sampleCategoryList);

            var sampleCategory = sampleCategoryList[3];
            var input = _fixture.GetSampleInput(sampleCategory.Id);

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Put<CategoryModelOutput>($"/categories/{sampleCategory.Id}",
                                                                            input);

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.OK);

            output.Should().NotBeNull();
            output!.Id.Should().Be(sampleCategory.Id);
            output!.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);

            var dbCategory = await _fixture.Persistence.GetById(sampleCategory.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async Task ErrorWhenNotFound()
        {
            // Arrange
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await _fixture.Persistence.InsertList(sampleCategoryList);

            var randomCategoryId = Guid.NewGuid();
            var input = _fixture.GetSampleInput(randomCategoryId);

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Put<ProblemDetails>($"/categories/{randomCategoryId}",
                                                                        input);

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.NotFound);

            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be((int)HttpStatusCode.NotFound);
            output.Detail.Should().Be($"Category '{randomCategoryId}' not found");
        }
    }
}
