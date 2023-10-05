using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture> { }

    public class CreateCategoryApiTestFixture : CategoryBaseFixture
    {
        public CreateCategoryInput GetSampleInput()
            => new(GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   GetRandomBoolean());

        public CreateCategoryInput GetInvalidInput()
            => new("", "", GetRandomBoolean());
    }
}
