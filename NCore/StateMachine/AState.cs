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
    /// </summary>
    /// 
    /// <typeparam name="A">
    /// 状態番号の型
    ///     例：enum型のEMainTaslState
    /// </typeparam>
    /// 
    /// <typeparam name="B">
    /// イベント番号の型
    ///     例：eunm型のEMainTaskEvent
    /// </typeparam>
    /// 
    /// <typeparam name="C">
    /// イベントハンドラのデリゲート
    ///     例：DMainTaskEventHandler
    /// </typeparam>
    public abstract class AState<A, B, C>
    {
        public readonly A Id;
        public readonly string Name;
        public Dictionary<B, C> EventHandlerTable = null;

        public AState(A id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.EventHandlerTable = new Dictionary<B, C>();
        }

    }
}
