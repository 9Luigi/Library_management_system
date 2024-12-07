using Microsoft.Extensions.Logging;
using Serilog;

public static class LoggerService
{
	/// <summary>
	/// Factory for creating loggers.
	/// </summary>
	public static ILoggerFactory LoggerFactory { get; }

	/// <summary>
	/// Static constructor to configure the logger.
	/// </summary>
	static LoggerService()
	{
		// Configure Serilog
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Information()
			.WriteTo.File(
				path: "logs/log.xml", // Logs will be written in an XML format
				rollingInterval: RollingInterval.Day, // Creates a new log file daily
				outputTemplate: "<?xml version=\"1.0\"?><log><date>{Timestamp:yyyy-MM-dd HH:mm:ss}</date><level>{Level:u}</level><message>{Message}</message><exception>{Exception}</exception></log>"
			)
			.CreateLogger();

		// Use Serilog as the logging provider
		LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
		{
			builder.ClearProviders(); // Remove default providers
			builder.AddSerilog();    // Add Serilog as the logging provider
		});
	}

	/// <summary>
	/// Creates a logger for the specified type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type for which the logger is created.</typeparam>
	/// <returns>An ILogger instance for the specified type.</returns>
	public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
}
