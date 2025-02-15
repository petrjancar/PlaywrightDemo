using Automation.Configuration.Logging;

namespace Automation.Utilities.Waiters;

/// <summary>
/// Helper class for polling waiting operations.
/// </summary>
/// <remarks>
/// Expected usage is mostly in Asserts.
/// </remarks>
public class PollingWaiter
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(100);

    private static async Task<bool> TryWaitInternalAsync(Func<Task<bool>> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string? timeoutMessage = null)
    {
        timeout ??= DefaultTimeout;
        pollingInterval ??= DefaultPollingInterval;
        timeoutMessage ??= $"The condition was not met within the timeout: {timeout}.";

        LoggingManager.LogMessage($"Waiting for the condition to be met with a timeout of {timeout} and a polling interval of {pollingInterval}.", typeof(PollingWaiter));

        using (var cts = new CancellationTokenSource((TimeSpan)timeout))
        {
            while (!cts.Token.IsCancellationRequested)
            {
                if (await condition())
                {
                    LoggingManager.LogMessage("The condition was met.", typeof(PollingWaiter));
                    return true;
                }
                try
                {
                    await Task.Delay((TimeSpan)pollingInterval, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        LoggingManager.LogMessage(timeoutMessage, typeof(PollingWaiter));
        return false;
    }

    /// <summary>
    /// Waits for a condition to be met.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="timeout">Default: 5s.</param>
    /// <param name="pollingInterval">Default: 100ms.</param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task WaitAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string? timeoutMessage = null)
    {
        if (!await TryWaitInternalAsync(() => Task.FromResult(condition()), timeout, pollingInterval, timeoutMessage))
        {
            throw new TimeoutException(timeoutMessage);
        }
    }

    /// <summary>
    /// Waits for an asynchronous condition to be met.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="timeout">Default: 5s.</param>
    /// <param name="pollingInterval">Default: 100ms.</param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task WaitAsync(Func<Task<bool>> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string? timeoutMessage = null)
    {
        if (!await TryWaitInternalAsync(condition, timeout, pollingInterval, timeoutMessage))
        {
            throw new TimeoutException(timeoutMessage);
        }
    }

    /// <summary>
    /// Tries to wait for a condition to be met.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="timeout">Default: 5s.</param>
    /// <param name="pollingInterval">Default: 100ms.</param>
    /// <returns>True if condition is met; false otherwise.</returns>
    public static Task<bool> TryWaitAsync(Func<bool> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string? timeoutMessage = null)
    {
        return TryWaitInternalAsync(() => Task.FromResult(condition()), timeout, pollingInterval, timeoutMessage);
    }

    /// <summary>
    /// Tries to wait for an asynchronous condition to be met.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="timeout">Default: 5s.</param>
    /// <param name="pollingInterval">Default: 100ms.</param>
    /// <returns>True if condition is met; false otherwise.</returns>
    public static Task<bool> TryWaitAsync(Func<Task<bool>> condition, TimeSpan? timeout = null, TimeSpan? pollingInterval = null, string? timeoutMessage = null)
    {
        return TryWaitInternalAsync(condition, timeout, pollingInterval, timeoutMessage);
    }
}
