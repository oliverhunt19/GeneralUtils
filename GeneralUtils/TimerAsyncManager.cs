namespace GeneralUtils
{
    public class TimerAsyncManager
    {
        private TimerAsync? timerAsync;

        private Func<CancellationToken, Task> scheduledAction;

        public bool IsRunning => timerAsync?.IsRunning ?? false;

        public TimerAsyncManager(Func<CancellationToken, Task> scheduledAction)
        {
            this.scheduledAction = scheduledAction;
        }

        public TimerAsyncManager(Func<Task> scheduledAction) : this((c)=> scheduledAction())
        {

        }

        public async Task Start(TimeSpan dueTime, TimeSpan period, bool canStartNextActionBeforePreviousIsCompleted = false)
        {
            await (timerAsync?.StopAsync()).NullableTask().ConfigureAwait(false);
            timerAsync?.Dispose();
            timerAsync = new TimerAsync(scheduledAction, dueTime, period, canStartNextActionBeforePreviousIsCompleted);
            await timerAsync.StartAsync().ConfigureAwait(false);
        }

        public Task Start(TimeSpan period, bool canStartNextActionBeforePreviousIsCompleted = false)
        {
            return Start(TimeSpan.Zero, period, canStartNextActionBeforePreviousIsCompleted);
        }

        //public Task Start()
        //{
        //    if(timerAsync == null)
        //    {
        //        throw new InvalidOperationException();
        //    }
        //    return timerAsync.StartAsync();
        //}

        public Task Stop()
        {
            return (timerAsync?.StopAsync()).NullableTask();
        }
    }
}
