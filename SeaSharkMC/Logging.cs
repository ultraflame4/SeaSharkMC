using System;
using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace SeaSharkMC;

public static class Logging
{
    
    public static LoggerConfiguration Config()
    {
        return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Literate,
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext:l} | {Prefix}{Message:lj}{NewLine}{Exception}");
    }
    

    public static ILogger Here<T>(
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        return Serilog.Log.Logger
                .ForContext("MemberName", memberName)
                .ForContext("FilePath", sourceFilePath)
                .ForContext("LineNumber", sourceLineNumber)
                .ForContext<T>();
    }
    public static ILogger Here(
        Type type,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        return Serilog.Log.Logger
                .ForContext("MemberName", memberName)
                .ForContext("FilePath", sourceFilePath)
                .ForContext("LineNumber", sourceLineNumber)
                .ForContext(type);
    }
}