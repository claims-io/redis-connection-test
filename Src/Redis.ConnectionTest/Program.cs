using System;
using System.Threading;
using ServiceStack.Redis;
using StackExchange.Redis;

namespace Redis.ConnectionTest
{
    class Program
    {
        private static bool _keepRunning = true;

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            var host = GetEnvVar("REDIS_HOST", "localhost");
            var port = GetEnvVar("REDIS_PORT", "6379");

            while (_keepRunning)
            {
                StackExchangeConnectAndTest(host, port);

                Thread.Sleep(TimeSpan.FromSeconds(5));

                Console.WriteLine("--------------------------------------");
                Console.WriteLine("--------------------------------------");

                ServiceStackConnectAndTest(host, port);

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

        }

        private static void ServiceStackConnectAndTest(string host, string port)
        {
            using (var redisManager = new RedisManagerPool($"{host}:{port}"))
            {
                var redis = redisManager.GetClient();
                var key = redis.ContainsKey("TEST_KEY");
                Console.WriteLine("SERVICE STACK DATABASE CONNECTED, TEST KEY RESULT: {0}", key);
            }

        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            _keepRunning = false;
        }

        static void StackExchangeConnectAndTest(string host, string port)
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { $"{host}:{port}" },
                Password = GetEnvVar("REDIS_PASSWORD")
            };

            var connection = ConnectionMultiplexer.Connect(configurationOptions, Console.Out);
            var database = connection.GetDatabase();

            var key = database.KeyExists("TEST_KEY");
            Console.WriteLine("STACK EXCHANGE DATABASE CONNECTED, TEST KEY RESULT: {0}", key);
        }

        static string GetEnvVar(string key, string defaultValue = null)
        {
            var envVar = Environment.GetEnvironmentVariable(key);

            return envVar ?? defaultValue;
        }
    }
}
