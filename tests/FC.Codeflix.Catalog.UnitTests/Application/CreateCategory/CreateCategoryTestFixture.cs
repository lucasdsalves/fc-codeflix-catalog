using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> {}

    public class CreateCategoryTestFixture : BaseFixture
    {
        public CreateCategoryTestFixture(): base() { }

        public CreateCategoryInput GetInput()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
                );

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

        public bool GetRandomBoolean()
        {
            return new Random().NextDouble() < 0.5;
        }              
    }
}
