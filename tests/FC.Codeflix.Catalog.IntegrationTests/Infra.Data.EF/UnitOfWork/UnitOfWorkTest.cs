using FC.Codeflix.Catalog.IntegrationTests.Base;
using UnitOfWorkInfra = FC.Codeflix.Catalog.Infra.Data.EF.UnitOfWork;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.UnitOfWork
{
    [Collection(nameof(UnitOfWorkTestFixture))]

    public class UnitOfWorkTest : BaseFixture
    {
        public UnitOfWorkTestFixture _fixture;

        public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(Commit))]
        [Trait("Integration/Infra.Data", "UnifOfWork - Persistence")]
        public async Task Commit()
        {
            // Arrange
            var dbContext = _fixture.CreateDbContext();
            var sampleCategoriesList = _fixture.GetSampleCategoriesList();

            await dbContext.AddRangeAsync(sampleCategoriesList);

            var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

            // Act
            await unitOfWork.Commit(CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);

            var savedCategories = assertDbContext.Categories
                                                 .AsNoTracking()
                                                 .ToList();

            // Assert
            savedCategories.Should().HaveCount(sampleCategoriesList.Count);
        }

        [Fact(DisplayName = nameof(Rollback))]
        [Trait("Integration/Infra.Data", "UnifOfWork - Persistence")]
        public async Task Rollback()
        {
            // Arrange
            var dbContext = _fixture.CreateDbContext();
            var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

            // Act
            var task = async () => await unitOfWork.Rollback(CancellationToken.None);

            // Assert
            await task.Should().NotThrowAsync();
        }
    }
}
