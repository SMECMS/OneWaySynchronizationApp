using OneWaySynchronizationConsoleApp.Interfaces;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#region Input Sanitization and Logging Setup

if (args.Length != 4) //Validate all 4 args are passed
{
    Console.WriteLine("You must pass all the 3 appropriate arguments");
    Console.WriteLine("Exiting Application");
    Environment.Exit(0);
}
try// validate LogFile path is valid
{
    Path.GetFullPath(args[3]); //if path is not valid GetFullPath will throw exception
    if (!Directory.Exists(args[3]))
    {
        throw new Exception();
    }
}
catch (Exception)
{
    Console.WriteLine("LogFile path is not valid");
    Console.WriteLine("Exiting Application");
    Environment.Exit(0);
}

string pathLogFile = args[3];

#if DEBUG
Log.Logger = new LoggerConfiguration() //Configure SeriLog
    .MinimumLevel.Debug()
    .WriteTo.File(pathLogFile, rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

using var loggerFactory = LoggerFactory.Create(static builder => //Configure Microsoft Ilogger and pass Serilog
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("OneWaySynchronizationConsoleApp", LogLevel.Debug)
        .AddSerilog(Log.Logger);
});
#else 
Log.Logger = new LoggerConfiguration() //Configure SeriLog
    .MinimumLevel.Information()
    .WriteTo.File(pathLogFile, rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

using var loggerFactory = LoggerFactory.Create(static builder => //Configure Microsoft Ilogger and pass Serilog
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("OneWaySynchronizationConsoleApp", LogLevel.Information)
        .AddConsole()
        .AddSerilog(Log.Logger);
});
#endif

ILogger logger = loggerFactory.CreateLogger<Program>();

logger.LogStartupMessage(pathLogFile);

try// validate Source path is valid
{
    Path.GetFullPath(args[0]); //if path is not valid GetFullPath will throw exception
    if (!Directory.Exists(args[0]))
    {
        throw new Exception();
    }
}
catch (Exception)
{
    logger.NotValidSourcePathMessage();
    logger.ExitApplicationMessage();
    Environment.Exit(0);
}
string pathSource = args[0];

try// validate Destination path is valid
{
    Path.GetFullPath(args[0]); //if path is not valid GetFullPath will throw exception
    if (!Directory.Exists(args[1]))
    {
        throw new Exception();
    }
}
catch (Exception)
{
    logger.NotValidDestinationPathMessage();
    logger.ExitApplicationMessage();
    Environment.Exit(0);
}
string pathDestination = args[1];

int intervalTime;
if (!int.TryParse(args[2], out intervalTime))
{
    logger.NotValidIntervalTimeMessage();
    logger.ExitApplicationMessage();
    Environment.Exit(0);
}

#endregion

#region Main Program

IOneWaySyncer oneWaySyncer = new OneWaySyncer(loggerFactory.CreateLogger<OneWaySyncer>());
var tokenSource = new CancellationTokenSource();
Task.Run(() =>  oneWaySyncer.StartSync(sourcePath: pathSource, destinationPath: pathDestination, intervalTime: intervalTime, cancellationToken: tokenSource.Token), tokenSource.Token);



logger.PressCToExitMessage();
while (Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.C)
{
}
logger.CKeyPressedMessage();
tokenSource.Cancel();
logger.ExitApplicationMessage();
Environment.Exit(0);
#endregion

