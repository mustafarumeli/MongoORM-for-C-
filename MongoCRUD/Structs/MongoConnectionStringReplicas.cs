namespace MongoCRUD.Structs
{
    public struct MongoConnectionStringReplicas
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return $"{nameof(Host)}: {Host}, {nameof(Port)}: {Port}";
        }
    }
}
