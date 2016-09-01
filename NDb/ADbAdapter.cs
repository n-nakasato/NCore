using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDb
{
    public abstract class ADbAdapter
    {
        string _Path = string.Empty;

        public string Path
        {
            get { return this._Path; }
            protected set { this._Path = value; }
        }

        public abstract void CreateFile(string path);

        public abstract void Open(string path);

        public abstract void Close();

        public abstract void CleanUp();

        public abstract string GetCreateTable(string tbl, CDbClm[] clm);

        public abstract string GetDropTable(string tbl);

        public abstract string GetInsert(string tbl, CDbClm[] clm);

        public abstract string GetUpdate(string tbl, CDbClm[] clm, string where);

        public abstract string GetDelete(string tbl, string where);
    }
}
