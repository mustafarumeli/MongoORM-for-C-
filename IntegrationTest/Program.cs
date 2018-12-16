using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoCRUD;
using MongoCRUD.Interfaces;

namespace IntegrationTest
{
    public class Entity : DbObjectSD
    {
        public string Name { get; set; }
    }
    class Program
    {
        private const string ConnectionString = "mongodb://ohm:741895623ohm@test-shard-00-00-imtir.mongodb.net:27017,test-shard-00-01-imtir.mongodb.net:27017,test-shard-00-02-imtir.mongodb.net:27017/test?ssl=true&replicaSet=test-shard-0&authSource=admin&retryWrites=true";

        static void Main(string[] args)
        {
            MongoDbConnection.InitializeAndStartConnection(connectionString: ConnectionString, databaseName:"MilitaryTCG");
            var crud = new Crud<Entity>();
            var entity = new Entity { Name = "Wow" };
            Console.Write(crud.CountAll);
            Console.ReadLine();
        }
    }
}
