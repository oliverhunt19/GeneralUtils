namespace GeneralUtils
{
    public static class NullableTaskHelpers
    {
        public static Task NullableTask(this Task? task)
        {
            return task.NullableTask(out _);
        }

        public static Task NullableTask(this Task? task, out bool IsNotNull)
        {
            if (task == null)
            {
                IsNotNull = false;
                return Task.CompletedTask;
            }
            else
            {
                IsNotNull = true;
                return task;
            }
        }

        public static Task<T?> NullableTask<T>(this Task<T?>? task)
        {
            return task.NullableTask(out _);
        }

        public static Task<T?> NullableTask<T>(this Task<T?>? task, out bool IsNotNull)
        {
            return task.NullableTask(default, out IsNotNull);
        }

        public static Task<T?> NullableTask<T>(this Task<T?>? task, T? defaultValue, out bool IsNotNull)
        {
            if(task == null)
            {
                IsNotNull = false;
                return Task.FromResult(defaultValue);
            }
            else
            {
                IsNotNull = true;
                return task;
            }
        }
    }
}
