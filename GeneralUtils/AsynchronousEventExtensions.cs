namespace GeneralUtils
{
    public delegate Task AsyncEvent<TSource, TEventArgs>(TSource source, TEventArgs eventArgs, CancellationToken cancellationToken = default);
    public static class AsynchronousEventExtensions
    {
        public static Task InvokeAsync<TSource, TEventArgs>(this AsyncEvent<TSource, TEventArgs>? handlers, TSource source, TEventArgs args, CancellationToken cancellationToken = default)
            where TEventArgs : EventArgs
        {
            if(handlers != null)
            {
                Delegate[] delegates = handlers.GetInvocationList();

                IEnumerable<AsyncEvent<TSource,TEventArgs>> asyncEvents = delegates.OfType<AsyncEvent<TSource,TEventArgs>>();
                //IEnumerable<Delegate> nonAsyncEvents = delegates.Where(x=>x is not  AsyncEvent<TSource, TEventArgs>);
                IEnumerable<Task> tasks = asyncEvents.Select(h => h.Invoke(source, args, cancellationToken));
                //nonAsyncEvents.Select(x=>x.)
                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }
    }
}
