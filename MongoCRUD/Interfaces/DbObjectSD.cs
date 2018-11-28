using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoCRUD.Interfaces
{
    public abstract class DbObjectSD : DbObject
    {
        public byte IsDeleted { get; set; }

        protected DbObjectSD()
        {
            IsDeleted = 0;
        }
    }
}
