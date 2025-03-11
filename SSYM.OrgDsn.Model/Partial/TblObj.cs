using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSYM.OrgDsn.Model
{
    public interface ITblObj
    {

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldNamObj { get; set; }
    }
    [MetadataType(typeof(ITblObj))]
    public partial class TblObj : IObjRst,ITblObj, IDataErrorInfo, INotifyDataErrorInfo 
    {
        public TblObj()
        {           
            dataErrorInfoSupport = new DataErrorInfoSupport(this);

        }
        bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {

                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public string FldNamActSrc
        {
            get
            {
                if (this.TblEvtRst != null)
                {
                    return this.TblEvtRst.TblAct.FldNamAct;
                }
                return string.Empty;
            }
        }

        public string FldNamNodSrc
        {
            get
            {
                if (this.TblEvtRst != null)
                {
                    return PublicMethods.ActivityPerformerName_951(this.TblEvtRst.FldCodAct);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// لیست فعالیتهایی که این آبجکت به آن ها وارد می شود
        /// </summary>
        public List<TblAct> ActTarget
        {
            get
            {
                if (this.TblWayIfrm_SndOut != null)
                {
                    return PublicMethods.DetectActTargetedBySpecificObjRst(this);
                }
                return null;
            }
        }

        /// <summary>
        /// لیست گره هایی که این آبجکت به آن ها وارد می شود
        /// </summary>
        public List<TblNod> NodTarget
        {
            get
            {
                if (this.TblWayIfrm_SndOut != null)
                {
                    return PublicMethods.DetectNodTargetedBySpecificObjRst(this);
                }
                return null;
            }
        }

        public bool HasDson
        {
            get;
            set;
        }

        public bool IsAdded
        {
            get;
            set;
        }



        public TblEvtRst EvtRst
        {
            get
            {
                return this.TblEvtRst;
            }
        }

        /// <summary>
        /// لیستی از نحوه های آگاهسازی این شیء نتیجه را برمیگرداند
        /// </summary>
        public List<IWayIfrm> WayIfrms
        {
            get
            {
                return this.TblWayIfrm_SndOut.ToList<IWayIfrm>();
            }
        }

        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldNamObj")
                {
                    return true;
                }
            }
            return false;
        }

        [NonSerialized]
        private DataErrorInfoSupport dataErrorInfoSupport;


        public string Error
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return null;
                }

                return dataErrorInfoSupport.Error;
            }
        }

        public string this[string memberName]
        {
            get
            {
                if (!shouldCheckErrors(memberName))
                {
                    return null;
                }

                return dataErrorInfoSupport[memberName];
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return;
            }

            dataErrorInfoSupport.RaiseErrorsChanged(propertyName);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged += value;
                }
            }
            remove
            {
                if (dataErrorInfoSupport != null)
                {
                    dataErrorInfoSupport.ErrorsChanged -= value;
                }
            }
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (!shouldCheckErrors(propertyName))
            {
                return null;
            }

            return dataErrorInfoSupport.GetErrors(propertyName);
        }

        public bool HasErrors
        {
            get
            {
                if (!shouldCheckErrors(null))
                {
                    return false;
                }

                return dataErrorInfoSupport.HasErrors;
            }
        }

        #endregion


        public string TypObj
        {
            get { return "TblObj"; }
        }

        public yWorks.yFiles.UI.Model.INode Shp
        {
            get;
            set;
        }

        public int CodObj
        {
            get { return this.FldCodObj; }
        }

        public yWorks.yFiles.UI.Model.INode Shp1
        {
            get;
            set;
        }


        public TblAct ActSrc
        {
            get { return this.TblEvtRst.TblAct; }
        }


        public string Name
        {
            get { return this.FldNamObj; }
        }


        List<TblEvtSrt> _evtSrtTarget;

        public List<TblEvtSrt> EvtSrtTarget
        {
            get
            {
                if (_evtSrtTarget == null)
                {
                    _evtSrtTarget = new List<TblEvtSrt>();

                    foreach (TblWayIfrm_SndOut item in this.TblWayIfrm_SndOut)
                    {
                        _evtSrtTarget.Add(item.TblWayAwr_RecvInt.TblEvtSrt);
                    }
                }

                return _evtSrtTarget;
            }
        }





    }
}
