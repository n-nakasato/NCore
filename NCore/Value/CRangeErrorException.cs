using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    /// <summary>
    /// 範囲外例外クラス
    /// 数値型の場合は範囲外、文字列型の場合は文字数範囲外のときに発生する
    /// Auther:n.n
    /// </summary>
    public class CRangeErrorException : Exception
    {
        public AGenValue Value;

        public CRangeErrorException(AGenValue value)
            : base(string.Format("{0} is Range Error! ({1} - {2})", value.DispName, value.Min, value.Max))
        {
            Value = value;
        }

        public string GetNewErrMsg(string fmt)
        {
            return string.Format(fmt, Value.DispName, Value.Min, Value.Max);
        }
    }
}
