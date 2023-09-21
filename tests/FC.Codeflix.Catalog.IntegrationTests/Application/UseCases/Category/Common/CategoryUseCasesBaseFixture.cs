using FC.Codeflix.Catalog.IntegrationTests.Base;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common
{
    public class CategoryUseCasesBaseFixture : BaseFixture
    {
        public DomainEntity.Category GetSampleCategory()
             => new(
                 GetValidCategoryName(),
                 GetValidCategoryDescription(),
                 GetRandomBoolean()
                 );

        public List<DomainEntity.Category> GetSampleCategoryList(int length = 5)
        {
            var listCategory = new List<DomainEntity.Category>();

            for (int i = 0; i < length; i++)
            {
                listCategory.Add(GetSampleCategory());
            }
            return listCategory;
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
    }
}
