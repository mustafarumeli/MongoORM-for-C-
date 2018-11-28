using System.Collections.Generic;
using System.Linq;
using MongoCRUD.Structs;

namespace MongoCRUD
{
    internal class MongoConnectionObjectBuilder
    {
        private MongoConnectionObject _mongoConnectionObject;
        private static MongoConnectionObject MongoConnectionObject;

        internal MongoConnectionObjectBuilder()
        {
            _mongoConnectionObject = new MongoConnectionObject();
            MongoConnectionObject = _mongoConnectionObject;
        }

        internal MongoConnectionObjectBuilder GiveUserName(string userName)
        {
            _mongoConnectionObject.UserName = userName;
            return this;
        }
        internal MongoConnectionObjectBuilder GivePassword(string password)
        {
            _mongoConnectionObject.Password = password;
            return this;
        }

        internal MongoConnectionObjectBuilder GiveHost(string host)
        {
            var mainIpConfig = _mongoConnectionObject.MainIpConfig;
            mainIpConfig.Host = host;
            return this;
        }
        internal MongoConnectionObjectBuilder GivePort(int port)
        {
            var mainIpConfig = _mongoConnectionObject.MainIpConfig;
            mainIpConfig.Port = port;
            return this;
        }
        internal MongoConnectionObjectBuilder GiveDatabaseName(string databaseName)
        {
            _mongoConnectionObject.DatabaseName = databaseName;
            return this;
        }
        internal MongoConnectionObjectBuilder GiveConnectionOptions(int w,string readPreference)
        {
            var connectionOptions = _mongoConnectionObject.ConnectionOption;
            connectionOptions.W = w;
            connectionOptions.ReadPreference = readPreference;
            return this;
        }
        internal MongoConnectionObjectBuilder GiveConnectionOptions(MongoConnectionStringOptions connectionStringOptions)
        {
            _mongoConnectionObject.ConnectionOption = (ConnectionOptions)connectionStringOptions;
            return this;
        }
        internal MongoConnectionObjectBuilder AddReplica(string host, int port)
        {
            _mongoConnectionObject.ReplicasIpConfig.Value.Add(new IpConfig
            {
                Host = host,
                Port = port
            });
            return this;
        }
        internal MongoConnectionObjectBuilder AddReplica(IEnumerable<MongoConnectionStringReplicas> connectionStringReplicas)
        {
            _mongoConnectionObject.ReplicasIpConfig.Value.AddRange(connectionStringReplicas.Select(x=>(IpConfig)x));
            return this;
        }
        public static explicit operator MongoConnectionObject(MongoConnectionObjectBuilder builder)
        {
            return MongoConnectionObject;
        }
    }
}