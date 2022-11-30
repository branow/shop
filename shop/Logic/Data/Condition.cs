using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Logic.Data
{
    class Condition
    {
        public DataField[] Fields { get; set; }
        public string Operator { get; set; }

        public Condition(params DataField[] fields)
        {
            this.Fields = fields;
            Operator = " and ";
        }

        public Condition(DataField[] fields, string _operator)
        {
            this.Fields = fields;
            Operator = _operator;
        }

        public string ToString(string sign)
        {
            string res = "where ";
            for (int i = 0; i < Fields.Length; i++)
            {
                res += Fields[i].ToString(sign + i);
                if (i < Fields.Length - 1)
                    res += Operator;
            }
            return res;
        }

        public override string ToString()
        {
            string res = "where ";
            for (int i = 0; i < Fields.Length; i++)
            {
                res += Fields[i].ToString();
                if (i < Fields.Length - 1)
                    res += Operator;
            }
            return res;
        }
    }
}
