using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.StateMachine
{
    /// <summary>
    /// イベントハンドラの引数
    /// Auther:n.n
    /// </summary>
    /// 
    /// <typeparam name="EventId">
    /// イベント番号の型
    ///     例：eunm型のEMainTaskEvent
    /// </typeparam>
    public class CEvent<EventId>
    {
        public ulong MsgNo = 1;
        public readonly string SrcName;
        public readonly EventId Id;
        public object Param;

        public CEvent(string srcName, EventId id, object param = null)
        {
            this.SrcName = srcName;
            this.Id = id;
            this.Param = param;
        }
    }
}
