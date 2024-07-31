namespace Authorizations.Tests.Common
{
    internal sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token)
            => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    internal sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync() => new(Task.Run(() => _enumerator.Dispose()));

        public ValueTask<bool> MoveNextAsync() => new(_enumerator.MoveNext());
    }

    internal sealed class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _innerQueryProvider;

        internal TestAsyncQueryProvider(IQueryProvider innerQueryProvider)
        {
            _innerQueryProvider = innerQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new TestAsyncEnumerable<TElement>(expression);

        public object? Execute(Expression expression) => _innerQueryProvider.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => _innerQueryProvider.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
        {
            var expectedResultType = typeof(TResult).GetGenericArguments().First();
            var executionResult = Execute(expression);
            var method = typeof(Task).GetMethod(nameof(Task.FromResult));

            return (TResult)method?
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult })!;
        }
    }
}
