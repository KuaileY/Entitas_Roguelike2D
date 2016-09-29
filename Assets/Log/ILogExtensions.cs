using System;
using log4net;

public static class ILogExtentions
{
  private static log4net.ILog log =
      log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private static void usage()
    {
        log.Debug("usage");
    }

  public static void Trace(this ILog log, string message, Exception exception)
  {
    log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
        log4net.Core.Level.Trace, message, exception);
  }

  public static void Trace(this ILog log, string message)
  {
    log.Trace(message, null);
  }

  public static void Verbose(this ILog log, string message, Exception exception)
  {
    log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
        log4net.Core.Level.Verbose, message, exception);
  }

  public static void Verbose(this ILog log, string message)
  {
    log.Verbose(message, null);
  }
}