namespace FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<OutputSearch<TAggregate>> Search(SearchInput input, CancellationToken cancellationToken);
    }
}
