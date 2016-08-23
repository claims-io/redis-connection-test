using System;
using System.Threading;
using StackExchange.Redis;

namespace Redis.ConnectionTest
{
    class Program
    {
        private static bool _keepRunning = true;

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            while (_keepRunning)
            {
                ConnectAndTest();
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _keepRunning = false;
        }

        static void ConnectAndTest()
        {
            var host = GetEnvVar("REDIS_HOST", "localhost");
            var port = GetEnvVar("REDIS_PORT", "6379");

            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{host}:{port}" },
                Password = GetEnvVar("REDIS_PASSWORD")
            };

            var connection = ConnectionMultiplexer.Connect(configurationOptions, Console.Out);
            var database = connection.GetDatabase();

            var key = database.KeyExists("TEST_KEY");
            Console.WriteLine("DATABASE CONNECTED, TEST KEY RESULT: {0}", key);
        }

        static string GetEnvVar(string key, string defaultValue = null)
        {
            var envVar = Environment.GetEnvironmentVariable(key);

            return envVar ?? defaultValue;
        }
    }
}
