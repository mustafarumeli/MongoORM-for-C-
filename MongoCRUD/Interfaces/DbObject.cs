using System;
using System.Collections.Generic;
using System.Text;

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

        public override string ToString()
        {
           
            var properties = GetType().GetProperties();
            var fields = GetType().GetFields();
            StringBuilder returnValue = new StringBuilder();
            WriteFields(properties);
            WriteFields(fields);
            returnValue.Remove(returnValue.Length - 2, 2);
            void WriteFields(IEnumerable<dynamic> enumerable)
            {
                foreach (var property in enumerable)
                {
                    var value = property.GetValue(this);
                    if (value != null)
                    {
                        returnValue.Append(property.Name + ":" + value + ", ");
                    }
                }
            }
            return returnValue.ToString();
        }
    }
}
