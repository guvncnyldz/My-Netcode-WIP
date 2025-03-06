using System;
using System.Threading;
using System.Threading.Tasks;

public static class AsyncExtensions
{
    //https://stackoverflow.com/a/24148785/8021855
    //It's not needed in .Net Core
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();

        using (cancellationToken.Register(source => ((TaskCompletionSource<bool>)source).TrySetResult(true), taskCompletionSource))
        {
            if (task != await Task.WhenAny(task, taskCompletionSource.Task))
            {
                throw new OperationCanceledException(cancellationToken);
            }
        }

        return task.Result;
    }
}