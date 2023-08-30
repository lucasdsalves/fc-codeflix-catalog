using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
    {
    }

    public class CategoryTestFixture : BaseFixture
    {
        public CategoryTestFixture() : base() { }

        public DomainEntity.Category GetValidCategory()
            => new DomainEntity.Category(GetValidCategoryName(),
                                         GetValidCategoryDescription());

        public string GetValidCategoryName()
        {
            var categoryName = "";

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName.Substring(0, 255);

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription.Substring(0, 10_000);

            return categoryDescription;
        }
    }
}