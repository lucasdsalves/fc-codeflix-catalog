using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.Common;

namespace FC.Codeflix.Catalog.IntegrationTests.Application.UseCases.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture> { }

    public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public UpdateCategoryInput GetCategoryInput(Guid? id)
        {
            var category = GetSampleCategory();

            return new UpdateCategoryInput(
                    id ?? Guid.NewGuid(),
                    category.Name,
                    category.Description,
                    category.IsActive
                );
        }
    }
}
