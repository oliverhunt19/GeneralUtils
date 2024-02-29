using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralUtils
{
    public class ActiveTimerAsyncCollection : Collection<TimerAsync>
    {

        protected override void InsertItem(int index, TimerAsync item)
        {
            item.TimerStopped += Item_TimerStopped;
            item.TimerStarted += Item_TimerStarted;
            base.InsertItem(index, item);
        }

        private void Item_TimerStarted(object? sender, EventArgs e)
        {
            TimerAsync timer = (TimerAsync)sender;
        }

        private void Item_TimerStopped(object? sender, EventArgs e)
        {
            TimerAsync timer = (TimerAsync)sender;
        }

        public TimerAsync Add(Func<CancellationToken, Task> scheduledAction, TimeSpan dueTime, TimeSpan period, bool canStartNextActionBeforePreviousIsCompleted = false)
        {
            TimerAsync timerAsync = new TimerAsync(scheduledAction, dueTime, period, canStartNextActionBeforePreviousIsCompleted);
            Add(timerAsync);
            return timerAsync;
        }

        public TimerAsync Add(Func<CancellationToken, Task> scheduledAction, TimeSpan period, bool canStartNextActionBeforePreviousIsCompleted = false)
        {
            return Add(scheduledAction, TimeSpan.Zero, period, canStartNextActionBeforePreviousIsCompleted);
        }

        public Task StopAll()
        {
            return Parallel.ForEachAsync(new List<TimerAsync>(this), async (x, y) =>
            {
                await x.StopAsync().ConfigureAwait(false);
            });
        }
    }

    public class TimerAsyncCollection : Collection<TimerAsync>
    {

    }
}
