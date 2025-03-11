using SSYM.OrgDsn.Model.Base;
using SSYM.OrgDsn.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using yWorks.yFiles.UI.Model;

namespace SSYM.OrgDsn.Model
{
    public partial class TblEvtRst : IEvt
    {
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

        INode shp;

        public INode Shp
        {
            get { return shp; }
            set
            {
                shp = value;
            }
        }

        /// <summary>
        /// این لیست، لیست اشیاء نتیجه واقعی این رخداد نتیجه نیست
        /// </summary>
        public ObservableCollection<IObjRst> ObjRsts { get; set; }

        /// <summary>
        /// این لیست اشیاء نتیجه واقعی این رخداد نتیجه است
        /// </summary>
        public List<IObjRst> ObjRst
        {
            get
            {
                return DetectObjRstOfEvtRst_572(this.GetContext<BPMNDBEntities>(), this);
            }
        }

        /// <summary>
        /// F572
        /// شناسایی اشیاء نتیجه یک رخداد نتیجه
        /// </summary>
        /// <param name="evtRst">رخداد نتیجه</param>
        /// <returns>اشیاء نتیجه</returns>
        private List<IObjRst> DetectObjRstOfEvtRst_572(BPMNDBEntities context, TblEvtRst evtRst)
        {
            List<IObjRst> lst = new List<IObjRst>();

            lst.AddRange(PublicMethods.DetectNewsOfEvtRst_573(context, evtRst));

            lst.AddRange(PublicMethods.DetectObjsOfEvtRst_574(context, evtRst));

            lst.AddRange(PublicMethods.DetectSbjOralsOfEvtRst_575(context, evtRst));

            return lst;
        }

        //public ObservableCollection<IWayIfrm> WayIfrmForNod
        //{
        //    get
        //    {
        //        return 
        //    }
        //}

        TblNod destNod;

        public TblNod DestNod
        {
            get { return destNod; }
            set
            {

                destNod = value;
                //OnPropertyChanged();
            }
        }


        /// <summary>
        /// نوع رخداد نتیجه
        /// </summary>
        public EvtRstType TypRst
        {
            get
            {
                return (EvtRstType)this.FldTypEvtRst;
            }
        }

        /// <summary>
        ///  نام رخداد نتیجه بر اساس نوع آن
        /// </summary>
        public string NameRst
        {
            get
            {
                using (BPMNDBEntities ctx = new BPMNDBEntities())
                {
                    return ctx.TblItmFixSfws.Single(m => m.FldCodSbj == 10 && m.FldCodItm == this.FldTypEvtRst).FldNamItm;
                }
            }
        }

        public void WayAwrs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        if (item.GetType() == typeof(TblObj))
                        {
                            this.TblObjs.Add(item as TblObj);
                        }
                        if (item.GetType() == typeof(TblSbjOral))
                        {
                            this.TblSbjOrals.Add(item as TblSbjOral);
                        }

                        if (item.GetType() == typeof(TblNew))
                        {
                            this.TblNews.Add(item as TblNew);
                        }

                    }
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        if (item.GetType() == typeof(TblObj))
                        {
                            this.TblObjs.Remove(item as TblObj);
                        }
                        if (item.GetType() == typeof(TblSbjOral))
                        {
                            this.TblSbjOrals.Remove(item as TblSbjOral);
                        }

                        if (item.GetType() == typeof(TblNew))
                        {
                            this.TblNews.Remove(item as TblNew);
                        }

                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }
            this.OnPropertyChanged("WayAwrs");
        }

        public void evtRst_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public INode ShpAgntEvtRst { get; set; }

    }
}
