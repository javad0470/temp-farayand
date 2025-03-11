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
    public interface ITblNew
    {

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        [StringLength(50, ErrorMessageResourceName = "MaxLength50", ErrorMessageResourceType = typeof(Model.Resources.ValidationResource))]
        string FldTtlNews { get; set; }
    }
    [MetadataType(typeof(ITblNew))]
    public partial class TblNew : ITblNew, IDataErrorInfo, INotifyDataErrorInfo, IObjRst
    {
        public TblNew()
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

        /// <summary>
        /// نام فعالیت تولید کننده خبر
        /// </summary>
        public string FldNamAct
        {
            get
            {
                if (this.TblEvtRst != null)
                {
                    return this.TblEvtRst.TblAct.FldNamAct;
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        /// <summary>
        /// نام گره تولید کننده خبر
        /// </summary>
        public string FldNamNod
        {
            get
            {
                if (this.TblEvtRst != null)
                {
                    return PublicMethods.ActivityPerformerName_951(this.TblEvtRst.FldCodAct);
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        /// <summary>
        /// لیست فعالیتهایی که این خبر به آن ها وارد می شود
        /// </summary>
        public List<TblAct> ActTarget
        {
            get
            {
                if (this.TblWayIfrm_News != null)
                {
                    return PublicMethods.DetectActTargetedBySpecificObjRst(this);
                }
                return null;
            }
        }

        /// <summary>
        /// لیست گره هایی که این خبر به آن ها وارد می شود
        /// </summary>
        public List<TblNod> NodTarget
        {
            get
            {
                if (this.TblWayIfrm_News != null)
                {
                    return PublicMethods.DetectNodTargetedBySpecificObjRst(this);
                }
                return null;
            }
        }

        #region ' Validation '

        private bool shouldCheckErrors(string property)
        {
            if (this.EntityState == System.Data.EntityState.Modified || this.EntityState == System.Data.EntityState.Detached)
            {
                if (property == null || property == "FldTtlNews")
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


        public TblAct ActSrc
        {
            get { return this.TblEvtRst.TblAct; }
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
                return this.TblWayIfrm_News.ToList<IWayIfrm>();
            }
        }


        public string TypObj
        {
            get { return "TblNew"; }
        }


        public yWorks.yFiles.UI.Model.INode Shp
        {
            get;
            set;
        }


        public int CodObj
        {
            get { return this.FldCodNews; }
        }


        public yWorks.yFiles.UI.Model.INode Shp1
        {
            get;
            set;
        }


        public string Name
        {
            get { return this.FldTtlNews; }
        }

        List<TblEvtSrt> _evtSrtTarget;

        public List<TblEvtSrt> EvtSrtTarget
        {
            get
            {

                if (_evtSrtTarget == null)
                {
                    _evtSrtTarget = new List<TblEvtSrt>();

                    foreach (TblWayIfrm_News item in this.TblWayIfrm_News)
                    {
                        _evtSrtTarget.Add(item.TblWayAwr_News.TblEvtSrt);
                    }
                }

                return _evtSrtTarget;
            }
        }
    }
}
