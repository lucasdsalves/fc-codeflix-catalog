using Repository = FC.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository
{
    [Collection(nameof(CategoryRepositoryTestFixture))]
    public class CategoryRepositoryTest
    {
        public CategoryRepositoryTestFixture _fixture;

        public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Insert))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Insert()
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategory = _fixture.GetSampleCategory();

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            // Act
            await categoryRepository.Insert(sampleCategory, CancellationToken.None);  
            await dbContext.SaveChangesAsync();

            // Assert
            var dbCategory = await dbContext.Categories.FindAsync(sampleCategory.Id);

            dbCategory.Should().NotBeNull();    
            dbCategory!.Name.Should().Be(sampleCategory.Name);
            dbCategory.Description.Should().Be(sampleCategory.Description); 
            dbCategory.IsActive.Should().Be(sampleCategory.IsActive);   
        }
    }
}
