using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoCRUD.Structs;

namespace MongoCRUD
{
    internal class MongoConnectionObject
    {
        public MongoConnectionObject(string userName, string password, IpConfig mainIpConfig,
            Lazy<List<IpConfig>> replicasIpConfig, string databaseName, ConnectionOptions connectionOption)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            MainIpConfig = mainIpConfig;
            ReplicasIpConfig = replicasIpConfig ?? throw new ArgumentNullException(nameof(replicasIpConfig));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            ConnectionOption = connectionOption;
        }

        public MongoConnectionObject()
        {
            UserName = string.Empty;
            Password = string.Empty;
            MainIpConfig = new IpConfig
            {
                Host = "127.0.0.1",
                Port = 27017
            };
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IpConfig MainIpConfig { get; set; }
        public Lazy<List<IpConfig>> ReplicasIpConfig { get; set; }
        public string DatabaseName { get; set; }
        public ConnectionOptions ConnectionOption { get; set; }


        public override string ToString()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return $"mongodb://{MainIpConfig.Host}:{MainIpConfig.Port}/{DatabaseName}";
            }
           
            else
            {
                return $"mongodb://{UserName}:{Password}@{MainIpConfig.Host}:{MainIpConfig.Port}/{DatabaseName}";
            }
        }

        public override bool Equals(object obj)
        {
            return obj is MongoConnectionObject @object &&
                   UserName == @object.UserName &&
                   Password == @object.Password &&
                   EqualityComparer<IpConfig>.Default.Equals(MainIpConfig, @object.MainIpConfig) &&
                   EqualityComparer<Lazy<List<IpConfig>>>.Default.Equals(ReplicasIpConfig, @object.ReplicasIpConfig) &&
                   DatabaseName == @object.DatabaseName &&
                   EqualityComparer<ConnectionOptions>.Default.Equals(ConnectionOption, @object.ConnectionOption);
        }
    }
    
    internal struct ConnectionOptions
    {
        public int W { get; set; }
        public string ReadPreference { get; set; }
        public static implicit operator MongoConnectionStringOptions(ConnectionOptions connectionStringOptions)
        {
            return new MongoConnectionStringOptions { ReadPreference = connectionStringOptions.ReadPreference, W = connectionStringOptions.W };
        }
        public static explicit operator ConnectionOptions(MongoConnectionStringOptions connectionStringOptions)
        {
            return new ConnectionOptions { ReadPreference = connectionStringOptions.ReadPreference, W = connectionStringOptions.W };
        }
    }
    internal struct IpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public static implicit operator MongoConnectionStringReplicas(IpConfig ipConfig)
        {
            return new MongoConnectionStringReplicas{Host = ipConfig.Host, Port = ipConfig.Port};
        }
        public static explicit operator IpConfig(MongoConnectionStringReplicas mongoConnectionStringReplicas)
        {
            return new IpConfig { Host = mongoConnectionStringReplicas.Host, Port = mongoConnectionStringReplicas.Port };
        }
    }
}
