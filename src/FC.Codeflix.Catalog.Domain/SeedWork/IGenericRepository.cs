﻿namespace FC.Codeflix.Catalog.Domain.SeedWork
{
    public interface IGenericRepository<TAggregate> : IRepository where TAggregate : AggregateRoot
    {
        public Task<TAggregate> Get(Guid id, CancellationToken cancellationToken);
        public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
        public Task Delete(TAggregate aggregate, CancellationToken cancellationToken);
        public Task<TAggregate> Update(TAggregate aggregate, CancellationToken cancellationToken);
    }
}
