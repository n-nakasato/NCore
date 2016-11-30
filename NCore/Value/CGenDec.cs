using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    /// <summary>
    /// decimal型の値クラス
    /// Auther:n.n
    /// </summary>
    public class CGenDec : AGenValue
    {
        private decimal _Value;
        private int _Degit;
        private string _DegitFmt;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="init"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="degit"></param>
        /// <param name="throwEx"></param>
        /// <param name="descriptionFmt"></param>
        public CGenDec(string id, string name, string unit, decimal init, decimal min, decimal max, int degit, bool throwEx, string descriptionFmt = "{0} = {1} - {2}{3}")
            : base(id, name, unit, min, max, throwEx, descriptionFmt)
        {
            if (0 > degit) throw new ArgumentOutOfRangeException("degit");
            this.Degit = degit;
            this.DegitFmt = string.Format("F{0}", this._Degit);

            if (min > max) throw new ArgumentOutOfRangeException("max");

            this.Value = init;
        }

        /// <summary>
        /// 小数点桁数
        /// </summary>
        public int Degit
        {
            get { return this._Degit; }
            set { this.SetProperty(ref this._Degit, value); }
        }

        /// <summary>
        /// 小数点桁数フォーマット
        /// </summary>
        public string DegitFmt
        {
            get { return this._DegitFmt; }
            set { this.SetProperty(ref this._DegitFmt, value); }
        }

        /// <summary>
        /// decimal型の値を設定/取得する
        /// </summary>
        public virtual decimal Value
        {
            get { return this._Value; }
            set
            {
                value = Math.Round(value, this._Degit);

                if (this.ThrouwException)
                {
                    if ((this.Min > value) || (this.Max < value))
                    {
                        throw new CRangeErrorException(this);
                    }
                }
                else
                {
                    value = Math.Max(value, this.Min);
                    value = Math.Min(value, this.Max);
                }

                this.SetProperty(ref this._Value, value);
                this.OnPropertyChanged("Text");
            }
        }

        /// <summary>
        /// 文字列に変換するプロパティ
        /// </summary>
        public override string Text
        {
            get { return this.Format(this.Value); }
            set 
            {
                try
                {
                    this.FromString(value);
                }
                catch (Exception ex)
                {
                    this.OnShowMessage(this, ex);
                }

                this.OnPropertyChanged("Text");
            }
        }

        /// <summary>
        /// 指定した値を、このインスタンスのフォーマットで返す
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string Format(decimal val)
        {
            return val.ToString(this._DegitFmt);
        }

        /// <summary>
        /// このインスタンスが保持する値をフォーマットした文字列で返す
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Format(this._Value);
        }

        /// <summary>
        /// 文字列からParseする
        /// </summary>
        /// <param name="str"></param>
        public void FromString(string str)
        {
            decimal val;
            bool ret = decimal.TryParse(str, out val);

            if (ret)
            {
                this.Value = val;
            }
            else
            {
                throw new CNotNumberException(this, str);
            }
        }

        protected override void DescriptionChange()
        {
            this.Description = string.Format(this.DescriptionFmt, this.Name, this.Min, this.Max, this.Unit);
            base.DescriptionChange();
        }
    }
}
