using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.StateMachine
{
    /// <summary>
    /// イベントハンドラの引数
    /// </summary>
    /// 
    /// <typeparam name="B">
    /// イベント番号の型
    ///     例：eunm型のEMainTaskEvent
    /// </typeparam>
    public class CEvent<B>
    {
        public ulong MsgNo = 1;
        public readonly string SrcName;
        public readonly B Id;
        public object Param;

        public CEvent(string srcName, B id, object param = null)
        {
            this.SrcName = srcName;
            this.Id = id;
            this.Param = param;
        }
    }
}
