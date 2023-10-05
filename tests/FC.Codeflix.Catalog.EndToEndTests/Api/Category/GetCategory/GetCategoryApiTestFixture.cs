using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory
{
    [CollectionDefinition(nameof(GetCategoryApiTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryApiTestFixture> { }

    public class GetCategoryApiTestFixture : CategoryBaseFixture
    {
    }
}
