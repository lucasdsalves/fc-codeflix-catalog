using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.IntegrationTests.Base;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture> { }

    public class CategoryRepositoryTestFixture : BaseFixture
    {
        public Category GetSampleCategory()
           => new(
               GetValidCategoryName(),
               GetValidCategoryDescription(),
               GetRandomBoolean()
               );

        public List<Category> GetSampleCategoriesList(int length = 5)
        => Enumerable.Range(1, length )
                     .Select(_ => GetSampleCategory())
                     .ToList();

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

        public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CodeflixCatalogDbContext(
                                new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                                    .UseInMemoryDatabase("integration-tests-db")
                                    .Options
                                );

            if (preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }
    }
}
