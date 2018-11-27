using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoCRUD
{
    public class MongoDbConnection
    {
        internal static bool InitializeAndStartConnection(string databaseName,string serverIP = "localhost", int port = 27017)
        {
            _client = new MongoClient($"mongodb://{serverIP}:{port}/{databaseName}");
            _database = _client.GetDatabase(databaseName);
            try
            {
                Database.RunCommand((Command<BsonDocument>)"{ping:1}");
            }
            catch (TimeoutException)
            {
                return false;
            }
            return true;
        }
        public static bool InitializeAndStartConnection(string databaseName,string serverIP = "localhost", int port = 27017,string userName = "", string password = "")
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) 
            {
                return InitializeAndStartConnection(databaseName,serverIP, port);
            }
            _client = new MongoClient($"mongodb://{userName}:{password}@{serverIP}:{port}/{databaseName}");
            _database = _client.GetDatabase(databaseName);
            try
            {
                Database.RunCommand((Command<BsonDocument>)"{ping:1}");
            }
            catch (TimeoutException)
            {
                return false;
            }
            return true;
        }

     
        private static IMongoClient _client;
        private static IMongoDatabase _database;
        internal static IMongoDatabase Database { get { return _database; } }
     

    }
}
