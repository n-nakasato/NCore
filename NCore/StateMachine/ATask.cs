using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NCore.StateMachine
{
    /// <summary>
    /// メッセージ受信ベースのタスククラス
    /// Auther:n.n
    /// </summary>
    /// <typeparam name="StateId">
    /// 状態番号の型
    ///     例：enum型のEMainTaslState
    /// </typeparam>
    /// <typeparam name="EventId">
    /// イベント番号の型
    ///     例：eunm型のEMainTaskEvent
    /// </typeparam>
    /// <typeparam name="EventHandler">
    /// イベントハンドラのデリゲート
    ///     例：DMainTaskEventHandler
    /// </typeparam>
    /// <typeparam name="StateClass">
    /// 状態オブジェクトの型
    ///     例：AState<StateId>を継承したAMainTaskState
    /// </typeparam>
    public abstract class ATask<StateId, EventId, EventHandler, StateClass>
        : AStateManager<StateId, EventId, EventHandler, StateClass>
        where StateClass : AState<StateId, EventId, EventHandler>
    {
        private Task _Task = null;
        private CancellationTokenSource _Source;
        private CancellationToken _Token;

        private Queue<CEvent<EventId>> _MsgQueue = new Queue<CEvent<EventId>>();

        private ManualResetEvent _Signal = new ManualResetEvent(false);

        /// <summary>
        /// タスクの動作周期
        /// </summary>
        public int TaskTick = 100;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        public ATask(string name)
            : base(name)
        {
        }

        /// <summary>
        /// タスクを開始する
        /// </summary>
        public virtual void Start()
        {
            if (null == this._Task)
            {
                this._Source = new CancellationTokenSource();
                this._Token = this._Source.Token;
                this._Task = new Task(new Action(this.TaskProc), this._Token);
                this._Task.Start();
            }
        }

        /// <summary>
        /// タスクを終了する
        /// </summary>
        public virtual void End()
        {
            if (null != this._Task)
            {
                this._Signal.Set();

                this._Source.Cancel();

                this._Task.Wait();

                this._Task.Dispose();
                this._Task = null;

                this._Source.Dispose();
                this._Source = null;
            }
        }

        /// <summary>
        /// タスク処理
        /// </summary>
        protected void TaskProc()
        {
            CEvent<EventId> msgObj = null;
            bool recv;

            while (!this._Token.IsCancellationRequested)
            {
                // イベントが無いときスリープする
                this._Signal.WaitOne(this.TaskTick);
                this._Signal.Reset();

                // メッセージキューからイベント受信
                recv = this.RecvMsgQueue(ref msgObj);

                if (recv)
                {
                    // イベント処理
                    this.OnEvent(msgObj);
                }
                else
                {
                    // アイドル処理
                    this.OnIdle();
                }
            }
        }

        /// <summary>
        /// アイドル処理
        /// </summary>
        protected abstract void OnIdle();

        /// <summary>
        /// メッセージキューにメッセージを設定する
        /// </summary>
        /// <param name="obj"></param>
        public virtual void SendMsgQueue(CEvent<EventId> obj)
        {
            lock (this._MsgQueue)
            {
                this._MsgQueue.Enqueue(obj);

                this._Signal.Set();
            }
        }

        /// <summary>
        /// メッセージキューからメッセージを取得する
        /// </summary>
        /// <returns></returns>
        protected virtual bool RecvMsgQueue(ref CEvent<EventId> obj)
        {
            bool ret;

            lock (this._MsgQueue)
            {
                if (0 < this._MsgQueue.Count)
                {
                    obj = this._MsgQueue.Dequeue();
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }

            return ret;
        }
    }
}
