using NLog;
using System;

namespace net.adamec.lib.common.core.logging
{
    public static class CommonLogging
    {
        public static ILogger CreateLogger(string categoryName)
        {
//            return LogManager.LogFactory.GetLogger<LoggerExt>(categoryName);
            return LogManager.LogFactory.GetLogger(categoryName);
        }

        public static ILogger CreateLogger(Type type)
        {
            return CreateLogger(type.FullName);
        }
        public static bool FatalFltr(this ILogger logger,Exception ex)
        {
            return false;
        }
        public static ILogger CreateLogger<T>()
        {
            return CreateLogger(typeof(T).FullName);
        }
        public static Exception Fatal<T>(this ILogger Logger, string executionId)
        {
            return new Exception(executionId);
        }
        public static Exception Fatal<T>(this ILogger Logger, string executionI,Exception exc)
        {
            return exc;
        }
        public static Exception Error<T>(this ILogger Logger, string executionId)
        {
            return new Exception(executionId);
        }
        public static void FatalCorr(this ILogger Logger, string executionId, string mess)
        {

        }
        public static Exception FatalCorr<T>(this ILogger Logger, string executionId, string mess)
        {
            return new Exception(mess);

        }
        public static Exception ErrorCorr<T>(this ILogger Logger, string executionId, string mess)
        {
            return new Exception(mess);

        }
        public static void WarnCorr(this ILogger Logger, string executionId, string mess)
        {

        }

        public static void ErrorCorr(this ILogger Logger, string executionId, string mess)
        {

        }
        public static void TraceCorr(this ILogger Logger,string executionId, string mess)
        {

        }
        public static void InfoCorr(this ILogger Logger, string executionId, string mess)
        {

        }
    }
}