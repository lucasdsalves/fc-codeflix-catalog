using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [Trait("EndToEnd/API", "Category/Create - Endpoints")]
        public async Task CreateCategory()
        {
            // Arrange
            var input = _fixture.GetSampleInput();

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Post<CategoryModelOutput>("/categories", input);

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);

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

        [Fact(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("EndToEnd/API", "Category/Create - Endpoints")]
        public async Task ThrowWhenCantInstantiateCategory()
        {
            // Arrange
            var input = _fixture.GetInvalidInput();

            // Act
            var (response, output) = await _fixture.ApiClient
                                                   .Post<ProblemDetails>("/categories", input);

            // Assert
            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

            output.Should().NotBeNull();
            output!.Title.Should().Be("One or more validation errors occured");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
