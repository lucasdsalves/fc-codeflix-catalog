using FC.Codeflix.Catalog.EndToEndTests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common
{
    public class CategoryBaseFixture : BaseFixture
    {
        public CategoryPersistence Persistence;

        public CategoryBaseFixture() : base(){ 
            Persistence = new CategoryPersistence(CreateDbContext());
        }

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

        public DomainEntity.Category GetSampleCategory()
            => new(GetValidCategoryName(),
                   GetValidCategoryDescription(),
                   GetRandomBoolean());

         public List<DomainEntity.Category> GetSampleCategoryList(int length = 5)
            => Enumerable.Range(1, length)
                         .Select(_ => new DomainEntity.Category(
                                                       GetValidCategoryName(),
                                                       GetValidCategoryDescription(),
                                                       GetRandomBoolean()
                                                       )
                         ).ToList();
    }
}
