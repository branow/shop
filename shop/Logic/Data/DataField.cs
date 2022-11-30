using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Data
{
    class DataField
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public DataField(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string ToString(string sign)
        {
            return $"{Name}={sign}";
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
