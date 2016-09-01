using NCore.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDb
{
    public class CDbClm
    {
        public string Name
        {
            get;
            protected set;
        }

        public string Type
        {
            get;
            protected set;
        }

        public AGenValue Value { get; set; }

        public CDbClm(string name, string type)
        {
            this.Name = name;
            this.Type = type;
            this.Value = null;
        }
    }
}
