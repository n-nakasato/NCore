using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDb
{
    public class CSqlite : ADbAdapter
    {
        SQLiteConnection _DbConn = null;
        public SQLiteConnection DbConn
        {
            get { return this._DbConn; }
            protected set { this._DbConn = value; }
        }

        SQLiteCommand _DbCmd = null;
        public SQLiteCommand DbCmd
        {
            get { return this._DbCmd; }
            protected set { this._DbCmd = value; }
        }

        bool _IsOpen = false;
        public bool IsOpen
        {
            get { return this._IsOpen; }
            protected set { this._IsOpen = value; }
        }

        public override void CreateFile(string path)
        {
            SQLiteConnection.CreateFile(path);
        }

        public override void Open(string path)
        {
            this.Path = path;

            try
            {
                this.DbConn = new SQLiteConnection("Data Source=" + this.Path);
                this.DbConn.Open();

                this.DbCmd = this.DbConn.CreateCommand();

                this.IsOpen = true;
            }
            catch(Exception ex)
            {
                Debug.Print(ex.Message);

                try
                {
                    this.Close();
                }
                catch
                {
                    // 何があっても知らない
                }

                throw ex;
            }
        }

        public override void Close()
        {
            try
            {
                if (null != this.DbCmd)
                {
                    this.DbCmd.Dispose();
                }
            }
            catch
            {
                // 何があっても知らない
            }
            finally
            {
                this.DbCmd = null;
            }

            try
            {
                if (null != this.DbConn)
                {
                    this.DbConn.Close();
                    this.DbConn.Dispose();
                }
            }
            catch
            {
                // 何があっても知らない
            }
            finally
            {
                this.DbConn = null;
            }

            this.IsOpen = false;
        }

        public override void CleanUp()
        {
            try
            {
                this.DbCmd.CommandText = "VACUUM";
                this.DbCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);

                try
                {
                    this.Close();
                }
                catch
                {
                    // 何があっても知らない
                }

                throw ex;
            }
        }

        public override string ToString()
        {
            if (this.IsOpen)
            {
                return string.Format("[Sqlite]{0}", this.Path);
            }
            else
            {
                return string.Format("[Sqlite]");
            }
        }

        public override string GetCreateTable(string tbl, CDbClm[] clm)
        {
            StringBuilder build = new StringBuilder();

            build.AppendFormat("CREATE TABLE {0}(", tbl);
            foreach (CDbClm now in clm)
            {
                build.AppendFormat("{0} {1},", now.Name, now.Type);
            }
            build.Length--;
            build.Append(")");

            return build.ToString();
        }

        public override string GetDropTable(string tbl)
        {
            return string.Format("DROP TABLE {0}", tbl);
        }

        public override string GetInsert(string tbl, CDbClm[] clm)
        {
            StringBuilder build = new StringBuilder();
            StringBuilder values = new StringBuilder();

            build.AppendFormat("INSERT INTO {0}(", tbl);
            values.AppendFormat(" VALUES(");

            foreach(CDbClm now in clm)
            {
                if (null != now.Value)
                {
                    build.AppendFormat("{0},", now.Name);
                    if ("TEXT" == now.Type)
                    {
                        values.AppendFormat("'{0}',", now.Value.Text);
                    }
                    else
                    {
                        values.AppendFormat("{0},", now.Value.Text);
                    }
                }
            }
            build.Length--;
            build.Append(")");
            values.Length--;
            values.Append(")");

            build.Append(values.ToString());

            return build.ToString();
        }

        public override string GetUpdate(string tbl, CDbClm[] clm, string where)
        {
            StringBuilder build = new StringBuilder();

            build.AppendFormat("UPDATE {0} SET ", tbl);

            for (int idx = 0; idx < clm.Length; idx++)
            {
                if ("TEXT" == clm[idx].Type)
                {
                    build.AppendFormat("{0} = '{1}',", clm[idx].Name, clm[idx].Value.Text);
                }
                else
                {
                    build.AppendFormat("{0} = {1},", clm[idx].Name, clm[idx].Value.Text);
                }
            }
            build.Length--;

            build.AppendFormat(" WHERE {0}", where);

            return build.ToString();
        }

        public override string GetDelete(string tbl, string where)
        {
            StringBuilder build = new StringBuilder();

            build.AppendFormat("DELETE FROM {0} WHERE {1}", tbl, where);

            return build.ToString();
        }
    }
}
