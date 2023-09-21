using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture> { }

    public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public CreateCategoryInput GetCategoryInput()
        {
            var category = GetSampleCategory();

            return new CreateCategoryInput(
                    category.Name,
                    category.Description,
                    category.IsActive
                );
        }
    }
}
