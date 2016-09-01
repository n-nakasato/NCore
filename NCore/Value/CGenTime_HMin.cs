using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCore.Value
{
    public class CGenTime_HMin : AGenValue
    {
        private TimeSpan _Value = TimeSpan.Zero;

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
        public CGenTime_HMin(string id, string name, string init, decimal min, decimal max, bool throwEx, string descriptionFmt = "{0} = {1} - {2}{3}")
            : base(id, name, "[h:min]", min, max, throwEx, descriptionFmt)
        {
            if (min > max) throw new ArgumentOutOfRangeException("max");

            this.Text = init;
        }

        public TimeSpan Value
        {
            get { return this._Value; }
            set 
            {
                decimal newVal = (decimal)value.TotalMinutes;

                if (this.ThrouwException)
                {
                    if ((this.Min > newVal) || (this.Max < newVal))
                    {
                        throw new CRangeErrorException(this);
                    }
                }
                else
                {
                    newVal = Math.Max(this.Min, newVal);
                    newVal = Math.Min(this.Max, newVal);
                }

                TimeSpan span = TimeSpan.FromMinutes((double)newVal);

                this.SetProperty(ref this._Value, span);
                this.OnPropertyChanged("Text");
            }
        }

        public override string Text
        {
            get { return this.CnvTimeSpan2TimeStr(this._Value); }
            set
            {
                try
                {
                    decimal timeVal = 0;
                    int tmp = 0;

                    var param = value.Split(new char[] { ':' });

                    if (2 == param.Length)
                    {
                        if (!int.TryParse(param[0], out tmp))
                        {
                            throw new CFormatException(this, value);
                        }
                        timeVal += tmp * 60;

                        if (!int.TryParse(param[1], out tmp))
                        {
                            throw new CFormatException(this, value);
                        }
                        timeVal += tmp;
                    }
                    else
                    {
                        if (!int.TryParse(param[0], out tmp))
                        {
                            throw new CFormatException(this, value);
                        }
                        timeVal += tmp;
                    }

                    this.Value = TimeSpan.FromMinutes((double)timeVal);
                }
                catch (Exception ex)
                {
                    this.OnShowMessage(this, ex);
                }

                this.OnPropertyChanged("Text");
            }
        }

        protected string CnvDec2TimeStr(decimal val)
        {
            TimeSpan span = TimeSpan.FromMinutes((double)val);

            return this.CnvTimeSpan2TimeStr(span);
        }

        protected string CnvTimeSpan2TimeStr(TimeSpan span)
        {
            return string.Format("{0}:{1}", Math.Floor(span.TotalHours), span.ToString("mm"));
        }

        protected override void DescriptionChange()
        {
            this.Description = string.Format(this.DescriptionFmt, this.Name, this.CnvDec2TimeStr(this.Min), CnvDec2TimeStr(this.Max), this.Unit);
            base.DescriptionChange();
        }
    }
}
