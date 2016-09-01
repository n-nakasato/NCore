using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
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
