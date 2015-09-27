using NLog;
using NLog.Config;
using NLog.Targets;

namespace Pizza.Framework.IntegrationTests.Base.Config
{
    public class LogManagerConfigurator
    {
        public static void ConfigureLogManager()
        {
            var console = new ColoredConsoleTarget();
            console.Layout = @"${date:format=HH\\:MM\\:ss} ${logger} ${message}";

            var normal = new FileTarget();
            normal.Layout = "${longdate}|${level:uppercase=true}|${callsite}|${newline}${message}${newline}";
            normal.FileName = "${basedir}/normallog.txt";

            var error = new FileTarget();
            error.Layout = "${message}";
            error.FileName = "${newline}${longdate}|${level:uppercase=true}|${callsite}|${newline}${message}${newline}${exception:innerFormat=Message,Type,StackTrace:format=Message,Type,StackTrace:maxInnerExceptionLevel=10}${newline}";

            var config = new LoggingConfiguration();
            config.AddTarget("console", console);
            config.AddTarget("file", normal);
            config.AddTarget("error", error);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, console));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, normal));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, error));

            LogManager.Configuration = config;
        }
    }
}