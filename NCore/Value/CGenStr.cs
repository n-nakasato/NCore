using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    public class CGenStr : AGenValue
    {
        private string _Value = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="init"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="degit"></param>
        /// <param name="throwEx"></param>
        /// <param name="descriptionFmt"></param>
        public CGenStr(string id, string name, string init, decimal min, decimal max, bool throwEx, string descriptionFmt = "{0} = {1} - {2}characters")
            : base(id, name, "", min, max, throwEx, descriptionFmt)
        {
            if (min > max) throw new ArgumentOutOfRangeException("max");

            this.Text = init;
        }

        public override string Text
        {
            get { return this._Value; }
            set
            {
                try
                {
                    if (this.ThrouwException)
                    {
                        if ((this.Min > value.Length) || (this.Max < value.Length))
                        {
                            throw new CRangeErrorException(this);
                        }
                    }
                    else
                    {
                        if (this.Min > value.Length)
                        {
                            for (int i = value.Length; i < this.Min; i++)
                            {
                                value += " ";
                            }
                        }

                        if (this.Max < value.Length)
                        {
                            value = value.Remove((int)this.Max);
                        }
                    }

                    this.SetProperty(ref this._Value, value);
                }
                catch (Exception ex)
                {
                    this.OnShowMessage(this, ex);
                }

                this.OnPropertyChanged("Text");
            }
        }

        protected override void DescriptionChange()
        {
            this.Description = string.Format(this.DescriptionFmt, this.Name, this.Min, this.Max);
            base.DescriptionChange();
        }
    }
}
