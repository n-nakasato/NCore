using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.StateMachine
{
    /// <summary>
    /// 状態スケルトンの骨組み
    /// 本クラスを継承し、イベント関数を定義する。
    /// Auther:n.n
    /// </summary>
    /// 
    /// <typeparam name="StateId">
    /// 状態番号の型
    ///     例：enum型のEMainTaslState
    /// </typeparam>
    /// 
    /// <typeparam name="EventId">
    /// イベント番号の型
    ///     例：eunm型のEMainTaskEvent
    /// </typeparam>
    /// 
    /// <typeparam name="EventHandler">
    /// イベントハンドラのデリゲート
    ///     例：DMainTaskEventHandler
    /// </typeparam>
    public abstract class AState<StateId, EventId, EventHandler>
    {
        public readonly StateId Id;
        public readonly string Name;
        public Dictionary<EventId, EventHandler> EventHandlerTable = null;

        public AState(StateId id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.EventHandlerTable = new Dictionary<EventId, EventHandler>();
        }

    }
}
