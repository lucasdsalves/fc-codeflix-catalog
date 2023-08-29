using FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
    {
    }

    public class CategoryTestFixture
    {

        public DomainEntity.Category GetValidCategory()
            => new DomainEntity.Category("Category name", "Category description");
    }
}