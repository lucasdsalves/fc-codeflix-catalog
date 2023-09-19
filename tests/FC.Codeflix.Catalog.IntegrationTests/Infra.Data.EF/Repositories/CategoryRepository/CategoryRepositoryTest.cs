using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
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
            var dbCategory = await (_fixture.CreateDbContext(true))
                                   .Categories.FindAsync(sampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(sampleCategory.Name);
            dbCategory.Description.Should().Be(sampleCategory.Description);
            dbCategory.IsActive.Should().Be(sampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(Get))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Get()
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategory = _fixture.GetSampleCategory();
            var sampleCategoryList = _fixture.GetSampleCategoriesList();

            sampleCategoryList.Add(sampleCategory);

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

            // Act
            await categoryRepository.Get(sampleCategory.Id, CancellationToken.None);

            // Assert
            var dbCategory = await dbContext.Categories.FindAsync(sampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Id.Should().Be(sampleCategory.Id);
            dbCategory!.Name.Should().Be(sampleCategory.Name);
            dbCategory.Description.Should().Be(sampleCategory.Description);
            dbCategory.IsActive.Should().Be(sampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(GetThrowIfNotFound))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task GetThrowIfNotFound()
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleId = Guid.NewGuid();

            var sampleCategoryList = _fixture.GetSampleCategoriesList();

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            // Act
            var task = async () =>
                await categoryRepository.Get(sampleId, CancellationToken.None);

            // Assert
            var dbCategory = await dbContext.Categories.FindAsync(sampleId);

            await task.Should().ThrowAsync<NotFoundException>()
                               .WithMessage($"Category '{sampleId}' not found");
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Update()
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategory = _fixture.GetSampleCategory();
            var newValuesForSampleCategory = _fixture.GetSampleCategory();
            var sampleCategoryList = _fixture.GetSampleCategoriesList();

            sampleCategoryList.Add(sampleCategory);

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            // Act
            sampleCategory.Update(newValuesForSampleCategory.Name, newValuesForSampleCategory.Description);
            await categoryRepository.Update(sampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var dbCategory = await (_fixture.CreateDbContext(true))
                                   .Categories.FindAsync(sampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Id.Should().Be(sampleCategory.Id);
            dbCategory!.Name.Should().Be(sampleCategory.Name);
            dbCategory.Description.Should().Be(sampleCategory.Description);
            dbCategory.IsActive.Should().Be(sampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(Delete))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task Delete()
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategory = _fixture.GetSampleCategory();
            var newValuesForSampleCategory = _fixture.GetSampleCategory();
            var sampleCategoryList = _fixture.GetSampleCategoriesList();

            sampleCategoryList.Add(sampleCategory);

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            // Act
            await categoryRepository.Delete(sampleCategory, CancellationToken.None);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            var dbCategory = await dbContext.Categories
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(c => c.Id == sampleCategory.Id);

            dbCategory.Should().BeNull();
        }

        [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        public async Task SearchReturnsListAndTotal()
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategoryList = _fixture.GetSampleCategoriesList();

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

            // Act
            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Items.Should().HaveCount(sampleCategoryList.Count);
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(sampleCategoryList.Count);
        }

        [Theory(DisplayName = nameof(SearchReturnsPaginated))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData(10, 1, 5, 5)]
        [InlineData(7, 2, 5, 2)]
        public async Task SearchReturnsPaginated(int quantityCategoriesToGenerate, int page, int perPage, int expectedQuantityItens)
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategoryList = _fixture.GetSampleCategoriesList(quantityCategoriesToGenerate);

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

            // Act
            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Items.Should().HaveCount(expectedQuantityItens);
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(quantityCategoriesToGenerate);
        }

        [Theory(DisplayName = nameof(SearchByText))]
        [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
        [InlineData("Action", 1, 5, 1, 1)]
        [InlineData("Horror", 1, 5, 3, 3)]
        [InlineData("Drama", 2, 5, 0, 1)]
        public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItensReturned, int expectedQuantityTotalItens)
        {
            // Arrange
            CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();

            var sampleCategoryList = _fixture.GetSampleCategoriesListhWithNames(new List<string>()
            {
                "Action",
                "Horror",
                "Horror - Robots",
                "Horror - Based On Real Facts",
                "Drama",
                "Sci-fi",
                "Comedy"
            });

            await dbContext.AddRangeAsync(sampleCategoryList);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var categoryRepository = new Repository.CategoryRepository(dbContext);

            var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

            // Act
            var output = await categoryRepository.Search(searchInput, CancellationToken.None);

            // Assert
            output.Should().NotBeNull();
            output.Items.Should().NotBeNull();
            output.Items.Should().HaveCount(expectedQuantityItensReturned);
            output.CurrentPage.Should().Be(searchInput.Page);
            output.PerPage.Should().Be(searchInput.PerPage);
            output.Total.Should().Be(expectedQuantityTotalItens);
        }
    }
}
