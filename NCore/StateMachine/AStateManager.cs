using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.StateMachine
{
    /// <summary>
    /// 状態を管理するクラス
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
    /// 
    /// <typeparam name="StateClass">
    /// 状態オブジェクトの型
    ///     例：AState<StateId, EventId, EventHandler>を継承したAMainTaskState
    /// </typeparam>
    public abstract class AStateManager<StateId, EventId, EventHandler, StateClass>
        where StateClass : AState<StateId, EventId, EventHandler>
    {
        public Dictionary<StateId, StateClass> StateTable;

        /// <summary>
        /// 管理名
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 現在の状態番号(クラス作成した人が責任を持ってデフォルト値を入れること)
        /// </summary>
        public StateId NowStateNo;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public AStateManager(string name)
        {
            this.Name = name;

            this.StateTable = new Dictionary<StateId, StateClass>();
        }

        /// <summary>
        /// 現在の状態
        /// </summary>
        public StateClass NowState
        {
            get { return this.StateTable[this.NowStateNo]; }
        }

        /// <summary>
        /// 状態オブジェクトを登録する
        /// </summary>
        /// <param name="state"></param>
        public void AddState(StateClass state)
        {
            this.StateTable.Add(state.Id, state);
        }

        /// <summary>
        /// ここでAStateの関数を呼ぶように実装する
        /// </summary>
        /// <param name="eventObj"></param>
        /// <returns></returns>
        protected abstract void OnEvent(CEvent<EventId> eventObj);
    }
}
