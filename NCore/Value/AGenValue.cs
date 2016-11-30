using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    /// <summary>
    /// 汎用値クラス
    /// string型への変換や、入力チェック機能を提供する
    /// 本クラスを継承して実際の値クラスを作成する
    /// Auther:n.n
    /// </summary>
    public abstract class AGenValue : BindableBase
    {
        private string _Id;
        private string _Name;
        private string _DispName;
        private string _Unit;
        private decimal _Min;
        private decimal _Max;
        private bool _ThrouwException;
        private string _Description;
        private string _DescriptionFmt;

        public event Action<AGenValue, Exception> ShowMessage;

        protected void OnShowMessage(AGenValue sender, Exception e)
        {
            if (null != this.ShowMessage)
            {
                this.ShowMessage(sender, e);
            }
        }

        public AGenValue(string id, string name, string unit, decimal min, decimal max, bool throwEx, string descFmt)
        {
            this.DescriptionFmt = descFmt;
            this.Id = id;
            this.Name = name;
            this.Unit = unit;
            this.DispName = name + unit;
            this.Min = min;
            this.Max = max;
            this.ThrouwException = throwEx;
        }

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return this._Id; }
            set { this.SetProperty(ref this._Id, value); }
        }

        /// <summary>
        /// 表示名前
        /// </summary>
        public string Name
        {
            get { return this._Name; }
            set { this.SetProperty(ref this._Name, value); }
        }

        /// <summary>
        /// 単位
        /// </summary>
        public string Unit
        {
            get { return this._Unit; }
            set { this.SetProperty(ref this._Unit, value); }
        }

        /// <summary>
        /// 表示名前(単位付き)
        /// </summary>
        public string DispName
        {
            get { return this._DispName; }
            set { this.SetProperty(ref this._DispName, value); }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public decimal Min
        {
            get { return this._Min; }
            protected set 
            {
                this.SetProperty(ref this._Min, value);
                this.MinMaxChanged();
            }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        public decimal Max
        {
            get { return this._Max; }
            protected set 
            {
                this.SetProperty(ref this._Max, value);
                this.MinMaxChanged();
            }
        }

        /// <summary>
        /// Min・Maxが変更されたときの処理
        /// </summary>
        protected virtual void MinMaxChanged()
        {
            bool back = this.ThrouwException;
            this.ThrouwException = false;

            this.Text = this.Text;

            this.ThrouwException = back;

            this.DescriptionChange();
        }

        /// <summary>
        /// このクラスが保持する値の文字列
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// 範囲外エラー時の挙動を設定する
        /// 
        /// true:例外を発生させる
        /// false:例外を発生させない
        /// </summary>
        public bool ThrouwException
        {
            get { return this._ThrouwException; }
            set { this.SetProperty(ref this._ThrouwException, value); }
        }

        /// <summary>
        /// 説明文
        /// </summary>
        public string Description
        {
            get { return this._Description; }
            set { this.SetProperty(ref this._Description, value); }
        }

        /// <summary>
        /// 説明文生成フォーマット
        /// </summary>
        public string DescriptionFmt
        {
            get { return this._DescriptionFmt; }
            set { this.SetProperty(ref this._DescriptionFmt, value); }
        }

        protected virtual void DescriptionChange()
        {
            // 継承して説明文を生成する処理を実装する
        }
    }
}
