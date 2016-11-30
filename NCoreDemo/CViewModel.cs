using Microsoft.Practices.Prism.Mvvm;
using NCore.Value;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NCoreDemo
{
    class CViewModel : BindableBase
    {
        protected CGenBool _IsItemEnable = new CGenBool("IsItemEnable", "IsItemEnable", false, false);

        public CGenBool IsItemEnable
        {
            get { return this._IsItemEnable; }
            set { this.SetProperty(ref this._IsItemEnable, value); }
        }

        protected CGenDec _ItemValue = new CGenDec("ItemValue", "ItemValue", "%", 0, -10, 110, 1, true);

        public CGenDec ItemValue
        {
            get { return this._ItemValue; }
            set { this.SetProperty(ref this._ItemValue, value); }
        }

        protected CGenStr _ItemName = new CGenStr("ItemName", "ItemName", "input here!", 0, 10, true);

        public CGenStr ItemName
        {
            get { return this._ItemName; }
            set { this.SetProperty(ref this._ItemName, value); }
        }

        public CViewModel()
        {
            ItemName.ShowMessage += ItemName_ShowMessage;
            ItemValue.ShowMessage += ItemValue_ShowMessage;
        }

        private void ItemName_ShowMessage(AGenValue arg1, Exception arg2)
        {
            MessageBox.Show(arg2.Message);
        }

        private void ItemValue_ShowMessage(AGenValue arg1, Exception arg2)
        {
            MessageBox.Show(arg2.Message);
        }
    }
}
