using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    public class CGenBool : AGenValue
    {
        bool _Value = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="init"></param>
        /// <param name="throwEx"></param>
        public CGenBool(string id, string name, bool init, bool throwEx)
            : base(id, name, "", 0, 0, throwEx, "{0} = True/False")
        {
            this.Value = init;
        }

        /// <summary>
        /// bool型の値を設定/取得する
        /// </summary>
        public virtual bool Value
        {
            get { return this._Value; }
            set
            {
                this.SetProperty(ref this._Value, value);
                this.OnPropertyChanged("Text");
            }
        }

        public override string Text
        {
            get { return this._Value.ToString(); }
            set
            {
                try
                {
                    this.SetProperty(ref this._Value, bool.Parse(value));
                }
                catch (Exception ex)
                {
                    this.OnShowMessage(this, ex);
                }

                this.OnPropertyChanged("Text");
            }
        }

        /// <summary>
        /// 説明文の変更処理
        /// </summary>
        protected override void DescriptionChange()
        {
            this.Description = string.Format(this.DescriptionFmt, this.Name);
        }
    }
}
