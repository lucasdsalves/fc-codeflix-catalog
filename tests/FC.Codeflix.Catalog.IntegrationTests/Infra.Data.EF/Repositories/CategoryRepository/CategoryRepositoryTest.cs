﻿using FC.Codeflix.Catalog.Application.Exceptions;
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

            var categoryRepository = new Repository.CategoryRepository(dbContext);

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
            var dbCategory = await dbContext.Categories.FindAsync(sampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Id.Should().Be(sampleCategory.Id);
            dbCategory!.Name.Should().Be(sampleCategory.Name);
            dbCategory.Description.Should().Be(sampleCategory.Description);
            dbCategory.IsActive.Should().Be(sampleCategory.IsActive);
        }
    }
}