using System;

using Cake.Core.Diagnostics;

namespace _build.Context;

public static class ContextLoggingExtensions
{
    public static void Info(this BuildContext context, string message)
    {
        context.Log.Information($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | INF | {message}");
    }
    public static void Warn(this BuildContext context, string message)
    {
        context.Log.Warning($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | WRN | {message}");
    }

    public static void Error(this BuildContext context, string message)
    {
        context.Log.Error($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | ERR | {message}");
    }

    public static void Debug(this BuildContext context, string message)
    {
        context.Log.Debug($"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fff} | DBG | {message}");
    }
}
