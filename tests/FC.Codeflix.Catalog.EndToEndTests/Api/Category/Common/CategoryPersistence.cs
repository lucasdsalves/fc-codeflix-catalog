using FC.Codeflix.Catalog.Infra.Data.EF;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common
{
    public class CategoryPersistence
    {
        private readonly CodeflixCatalogDbContext _context;

        public CategoryPersistence(CodeflixCatalogDbContext context)
        {
            _context = context;
        }

        public async Task<DomainEntity.Category?> GetById(Guid id)
        {
            return _context.Categories.AsNoTracking()
                                      .FirstOrDefault(c => c.Id == id);
        }
    }
}
