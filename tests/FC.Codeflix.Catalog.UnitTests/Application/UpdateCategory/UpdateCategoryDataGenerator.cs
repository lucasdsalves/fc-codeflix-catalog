using UpdateCategoryUseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    public class UpdateCategoryDataGenerator
    {
        public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 5)
        {
            var fixture = new UpdateCategoryTestFixture();

            for (int i = 0; i < times; i++)
            {
                var sampleCategory = fixture.GetSampleCategory();

                var sampleInput = fixture.GetValidInput(sampleCategory.Id);

                yield return new object[]
                {
                    sampleCategory, sampleInput
                };
            }
        }
    }
}
