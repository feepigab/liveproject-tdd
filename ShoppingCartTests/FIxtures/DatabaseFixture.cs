using AutoMapper;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.Mapping;
using System.Diagnostics;

namespace ShoppingCartServiceTests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private Process? _process;
        private readonly string _connectionString = "mongodb://localhost:1111";
        private readonly string _imageName = "mongo_test";
        public IMapper Mapper { get; }

        public ShoppingCartDatabaseSettings GetDatabaseSettings()
        {
            return new ShoppingCartDatabaseSettings
            {
                CollectionName = "Shopping Cart",
                ConnectionString = _connectionString,
                DatabaseName = "ShoppingCartDb"
            };
        }

        public DatabaseFixture()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            Mapper = config.CreateMapper();

            _process = Process.Start("docker", $"run --name {_imageName} -p 1111:27017 mongo");
            var started = WaitForDbConnection(_connectionString, "admin");
            if (!started)
            {
                throw new Exception("MongoDB startup failed.");
            }
        }

        public void Dispose()
        {
            Console.Out.WriteLine("Dispose called");
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }

            var processStop = Process.Start("docker", $"stop {_imageName}");
            processStop?.WaitForExit();
            var processRm = Process.Start("docker", $"rm {_imageName}");
            processRm?.WaitForExit();
        }

        private static bool WaitForDbConnection(string connectionString, string dbName)
        {
            var probeTask = Task.Run(() =>
            {
                var isAlive = false;
                var client = new MongoClient(connectionString);

                for (var i = 0; i < 3000; i++)
                {
                    client.GetDatabase(dbName);
                    var server = client.Cluster.Description.Servers.FirstOrDefault();
                    isAlive = server != null &&
                    server.HeartbeatException == null &&
                    server.State == MongoDB.Driver.Core.Servers.ServerState.Connected;

                    if (isAlive) { break; }

                    Thread.Sleep(100);
                }

                return isAlive;
            });

            probeTask.Wait();

            return probeTask.Result;
        }
    }
}
