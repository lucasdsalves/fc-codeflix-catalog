using System;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryApiTestFixture))]

    public class DeleteCategoryApiTest
    {
        private readonly DeleteCategoryApiTestFixture _fixture;

        public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async Task DeleteCategory()
        {
            // Arrange
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await _fixture.Persistence.InsertList(sampleCategoryList);

            var sampleCategory = sampleCategoryList[3];

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Delete<object>($"/categories/{sampleCategory.Id}");

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);

            output.Should().BeNull();

            var persistenceCategory = await _fixture.Persistence.GetById(sampleCategory.Id);    
            persistenceCategory.Should().BeNull();
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async Task ErrorWhenNotFound()
        {
            // Arrange
            var sampleCategoryList = _fixture.GetSampleCategoryList();
            await _fixture.Persistence.InsertList(sampleCategoryList);

            var randomCategoryId = Guid.NewGuid();

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Delete<ProblemDetails>($"/categories/{randomCategoryId}");

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);

            output.Should().NotBeNull();
            output!.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be((int)HttpStatusCode.NotFound);
            output.Detail.Should().Be($"Category '{randomCategoryId}' not found");
        }
    }
}
