using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    /// <summary>
    /// 非数値例外クラス
    /// 数値型のクラスで数値以外が入力されたときに発生する
    /// Auther:n.n
    /// </summary>
    public class CNotNumberException : Exception
    {
        public CGenDec Value;
        public string InputStr;

        public CNotNumberException(CGenDec decValue, string value)
            : base(string.Format("{0} is Number Only! ({1})", decValue.DispName, value))
        {
            Value = decValue;
            InputStr = value;
        }

        public string GetNewErrMsg(string fmt)
        {
            return string.Format(fmt, Value.DispName, InputStr);
        }
    }
}
