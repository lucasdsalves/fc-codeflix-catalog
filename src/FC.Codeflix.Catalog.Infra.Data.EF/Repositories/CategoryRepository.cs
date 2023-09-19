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
            var toSkip = (input.Page - 1) * input.PerPage;

            var query = _categories.AsNoTracking();

            query = AddOrderToQuery(query, input.OrderBy, input.Order);

            if (!string.IsNullOrWhiteSpace(input.Search))
                query = query.Where(c => c.Name.Contains(input.Search));

            var total = await query.CountAsync();

            var items = await query.Skip(toSkip)
                                   .Take(input.PerPage)
                                   .ToListAsync();

            return new OutputSearch<Category>(input.Page, input.PerPage, total, items);
        }

        public Task<Category> Update(Category aggregate, CancellationToken cancellationToken)
        {
            _categories.Update(aggregate);

            return Task.FromResult(aggregate);
        }

        private IQueryable<Category> AddOrderToQuery(IQueryable<Category> query, string orderProperty, SearchOrder order)
            => (orderProperty.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => query.OrderBy(c => c.Name),
                ("name", SearchOrder.Desc) => query.OrderByDescending(c => c.Name),
                ("id", SearchOrder.Asc) => query.OrderBy(c => c.Id),
                ("id", SearchOrder.Desc) => query.OrderByDescending(c => c.Id),
                _ => query.OrderBy(c => c.Name)
            };
    }
}
