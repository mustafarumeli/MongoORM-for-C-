using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoCRUD.Interfaces
{
    public abstract class DbObject
    {
        public string _id { get; set; }
        public DateTime CreationDate { get; set; }

        protected DbObject()
        {
            _id = Guid.NewGuid().ToString();
            CreationDate = DateTime.Now;
        }
    }
}
