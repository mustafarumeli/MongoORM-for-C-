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
        /// <summary>
        /// Initializes a database connection by the given parameters. If databaseName is empty then use the 'admin' database.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="serverIP"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="connectionStringOptions"></param>
        /// <param name="connectionStringReplicas"></param>
        /// <returns></returns>
        public static bool InitializeAndStartConnection(string databaseName = "",
            string serverIP = "localhost",
            int port = 27017,
            string userName = "", 
            string password = "",
            Dictionary<string,string> connectionStringOptions = null,
            IEnumerable<MongoConnectionStringReplicas> connectionStringReplicas = null)
        {
            if (string.IsNullOrWhiteSpace(databaseName) == true) databaseName = "admin";
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
        /// <summary>
        /// Lets you to switch between databases.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static bool SwitchDatabase(string databaseName)
        {
            if (_client == null)
            {
                throw new MongoDbDatabaseConnectionNotEstablised();
            }

            _database = _client.GetDatabase(databaseName);
            try
            {
                Database.RunCommand((Command<BsonDocument>)"{ping:1}");
            }
            catch (TimeoutException)
            {
                throw new MongoDbDatabaseConnectionNotEstablised();
                //todo Add Exception to the name = MongoDbDatabaseConnectionNotEstablishedException
            }
            return true;
        }
        /// <summary>
        /// Initializes a database connection by the given connection string. If databaseName is empty then use the 'admin' database.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public static bool InitializeAndStartConnection(string connectionString, string databaseName = "")
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(string.IsNullOrWhiteSpace(databaseName) == false ? databaseName : "admin");
            try
            {
                Database.RunCommand((Command<BsonDocument>)"{ping:1}");
            }
            catch (TimeoutException)
            {
                throw new MongoDbDatabaseConnectionNotEstablised();
                //todo Add Exception to the name = MongoDbDatabaseConnectionNotEstablishedException
            }
            return true;
        }

        private static IMongoClient _client;
        private static IMongoDatabase _database;
        internal static IMongoDatabase Database => _database;
    }
}
