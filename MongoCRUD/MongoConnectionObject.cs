using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoCRUD.Structs;

namespace MongoCRUD
{
    public class MongoConnectionObject
    {
        public MongoConnectionObject(string userName, string password, IpConfig mainIpConfig,
            List<IpConfig> replicasIpConfig, string databaseName, Dictionary<string, string> connectionOption)
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
            ReplicasIpConfig = new List<IpConfig>();
            ConnectionOption = new Dictionary<string, string>();
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public IpConfig MainIpConfig { get; set; }
        public List<IpConfig> ReplicasIpConfig { get; set; }
        public string DatabaseName { get; set; }
        public Dictionary<string, string> ConnectionOption { get; set; }


        public override string ToString()
        {
            StringBuilder connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append($"mongodb://");
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                connectionStringBuilder.Append($"{UserName}:{Password}@");
            }

            connectionStringBuilder.Append($"{MainIpConfig.Host}:{MainIpConfig.Port}");
            if (ReplicasIpConfig.Count > 0)
            {
                connectionStringBuilder.Append($",{ReplicaIpString}");
            }

            if (!string.IsNullOrEmpty(DatabaseName) || ConnectionOption.Count > 0)
            {
                connectionStringBuilder.Append("/");
                if (!string.IsNullOrEmpty(DatabaseName))
                {
                    connectionStringBuilder.Append(DatabaseName);
                }

                if (ConnectionOption.Count > 0)
                {
                    connectionStringBuilder.Append("?");
                    foreach (var option in ConnectionOption)
                    {
                        connectionStringBuilder.Append(option.Key + "=" + option.Value + "&");
                    }

                    connectionStringBuilder.Remove(connectionStringBuilder.Length - 1, 1);
                }
            }

            return connectionStringBuilder.ToString();
        }

        private string ReplicaIpString
        {
            get
            {
                StringBuilder replicaIp = new StringBuilder();
                for (var i = 0; i < ReplicasIpConfig.Count - 1; i++)
                {
                    var ipConfig = ReplicasIpConfig[i];
                    replicaIp.Append(ipConfig.Host + ":" + ipConfig.Port + ",");
                }

                var last = ReplicasIpConfig[ReplicasIpConfig.Count - 1];
                replicaIp.Append(last.Host + ":" + last.Port);
                return replicaIp.ToString();
            }
        }

        public struct IpConfig
        {
            public string Host { get; set; }
            public int Port { get; set; }

            public static implicit operator MongoConnectionStringReplicas(IpConfig ipConfig)
            {
                return new MongoConnectionStringReplicas {Host = ipConfig.Host, Port = ipConfig.Port};
            }

            public static explicit operator IpConfig(MongoConnectionStringReplicas mongoConnectionStringReplicas)
            {
                return new IpConfig
                    {Host = mongoConnectionStringReplicas.Host, Port = mongoConnectionStringReplicas.Port};
            }
        }
    }
}
