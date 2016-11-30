using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    /// <summary>
    /// フォーマットエラー例外クラス
    /// 入力チェックで引っかかると発生する
    /// Auther:n.n
    /// </summary>
    public class CFormatException : Exception
    {
        public AGenValue Value;
        public string InputStr;

        public CFormatException(AGenValue decValue, string value)
            : base(string.Format("{0} is Format Error! ({1})", decValue.DispName, value))
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
