using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoCRUD.Structs
{
    public struct MongoConnectionStringOptions
    {
        public int W { get; set; }
        public string ReadPreference { get; set; }

        public override string ToString()
        {
            return $"{nameof(W)}: {W}, {nameof(ReadPreference)}: {ReadPreference}";
        }


    }
}
