using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CodeflixCatalogDbContext _context;
        private DbSet<Category> _categories => _context.Set<Category>();

        public CategoryRepository(CodeflixCatalogDbContext context)
        {
            _context = context;
        }

        public async Task Insert(Category aggregate, CancellationToken cancellationToken)
        {
            await _categories.AddAsync(aggregate, cancellationToken);
        }

        public Task Delete(Category aggregate, CancellationToken cancellationToken)
        {
            return Task.FromResult(_categories.Remove(aggregate));
        }

        public async Task<Category> Get(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categories.AsNoTracking()
                                            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (category is null)
                throw new NotFoundException($"Category '{id}' not found");

            return category;
        }

        public async Task<OutputSearch<Category>> Search(SearchInput input, CancellationToken cancellationToken)
        {
            var total = await _categories.CountAsync();
            var items = await _categories.ToListAsync();

            return new OutputSearch<Category>(input.Page, input.PerPage, total, items);
        }

        public Task<Category> Update(Category aggregate, CancellationToken cancellationToken)
        {
            _categories.Update(aggregate);

            return Task.FromResult(aggregate);
        }
    }
}
