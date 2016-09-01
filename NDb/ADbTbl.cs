using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDb
{
    public abstract class ADbTbl
    {
        public string Name
        {
            get;
            set;
        }

        public CDbClm[] Columns
        {
            get;
            protected set;
        }
    }
}
