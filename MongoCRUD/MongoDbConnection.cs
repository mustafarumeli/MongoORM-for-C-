using System;
using System.Collections.Generic;
using MongoCrudExceptionHandling.Exceptions;
using MongoCRUD.Structs;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoCRUD
{
    public class MongoDbConnection
    {
        public static bool InitializeAndStartConnection(string databaseName,
            string serverIP = "localhost",
            int port = 27017,
            string userName = "", 
            string password = "",
            Dictionary<string,string> connectionStringOptions = null,
            IEnumerable<MongoConnectionStringReplicas> connectionStringReplicas = null)
        {
            MongoConnectionObjectBuilder builder = new MongoConnectionObjectBuilder();
            MongoConnectionObject mObject = (MongoConnectionObject)builder.GiveDatabaseName(databaseName)
                .GiveHost(serverIP)
                .GivePort(port)
                .GiveUserName(userName)
                .GivePassword(password);

            _client = new MongoClient(mObject.ToString());
            _database = _client.GetDatabase(databaseName);
            try
            {
                Database.RunCommand((Command<BsonDocument>)"{ping:1}");
            }
            catch (TimeoutException)
            {
                throw new MongoDbDatabaseConnectionNotEstablised(databaseName,serverIP,port); //todo Add Exception to the name = MongoDbDatabaseConnectionNotEstablishedException
            }
            return true;
        }


        private static IMongoClient _client;
        private static IMongoDatabase _database;
        internal static IMongoDatabase Database => _database;
    }
}
