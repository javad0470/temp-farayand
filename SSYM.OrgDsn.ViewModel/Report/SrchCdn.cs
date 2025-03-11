using Microsoft.Practices.Prism.ViewModel;
using SSYM.OrgDsn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.ViewModel.Report
{
    public class SrchCdn<T> : NotificationObject
    {
        #region ' Fields '

        T _selectedCdn;

        #endregion

        #region ' Initialaizer '

        public SrchCdn()
        {
        }

        #endregion

        #region ' Properties / Commands '

        /// <summary>
        /// شرط انتخاب شده
        /// بایند میشود
        /// </summary>
        public T SelectedCdn
        {
            get
            {
                return _selectedCdn;
            }
            set
            {
                _selectedCdn = value;
                AdjustView();
            }
        }

        private void AdjustView()
        {
            TypeAttribute ta = (TypeAttribute)TypeOfEnum.GetMember(_selectedCdn.ToString())[0].GetCustomAttributes(typeof(TypeAttribute), false).First();

            if (ta.ValueType == typeof(bool))
            {
                CheckBoxVisible = true;
                IntVisible = ComboVisible = TextBoxVisible = false;
                SelectedValueType = ta.ValueType;
            }
            else
                if (ta.ValueType.IsEnum)
                {
                    ComboVisible = true;
                    IntVisible = CheckBoxVisible = TextBoxVisible = false;
                    SelectedValueType = ta.ValueType;
                    RaisePropertyChanged("SelectedValueType");
                }
                else
                    if (ta.ValueType == typeof(int))
                    {
                        IntVisible = true;
                        TextBoxVisible = CheckBoxVisible = ComboVisible = false;
                        SelectedValueType = ta.ValueType;
                    }
                    else if (ta.ValueType == typeof(string))
                    {
                        TextBoxVisible = true;
                        IntVisible = CheckBoxVisible = ComboVisible = false;
                        SelectedValueType = ta.ValueType;
                    }

            RaisePropertyChanged("CheckBoxVisible", "ComboVisible", "TextBoxVisible", "IntVisible");
        }

        public Type TypeOfEnum
        {
            get
            {
                return typeof(T);
            }
        }

        public Type SelectedValueType { get; set; }

        private Type _selectedValueType;

        public int ComboSelectedIndex
        {
            get
            {
                return 0;
            }
        }

        public bool CheckBoxVisible { get; set; }

        public bool ComboVisible { get; set; }

        public bool TextBoxVisible { get; set; }

        public bool IntVisible { get; set; }

        public int GrpCdn { get; set; }

        /// <summary>
        /// bind mishavad
        /// </summary>
        /// 
        object _value;


        public bool CheckBoxValue { get; set; }

        public int IntValue { get; set; }

        public System.Enum EnumValue { get; set; }

        public string StrValue { get; set; }

        public object Value
        {
            get
            {
                if (CheckBoxVisible)
                {
                    return CheckBoxValue;
                }
                if (TextBoxVisible)
                {
                    return StrValue;
                }
                if (ComboVisible)
                {
                    return EnumValue;
                }
                if (IntVisible)
                {
                    return IntValue;
                }
                return null;
            }
        }

        public TResult GetValue<TResult>()
        {
            if (Value == null)
            {
                return default(TResult);
            }

            return (TResult)Value;
        }
        #endregion

        #region ' Public Methods '

        #endregion

        #region ' Private Methods '

        #endregion

        #region ' Events '

        #endregion


    }
}
