using SSYM.OrgDsn.Model;
using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace SSYM.OrgDsn.ViewModel.Base
{
    public class UserControlViewModel : BaseViewModel, IDisposable
    {
        //private BPMNDBEntities context;

        bool _isEnabled;

        public UserControlViewModel()
        {
            this.IsEnabled = true;
            Initialiaze();
        }

        public UserControlViewModel(BPMNDBEntities context)
        {
            bpmnEty = context;
            Initialiaze();
        }


        public UserControlViewModel(BPMNDBEntities context, EntityObject obj)
        {
            bpmnEty = context;
            Entity = obj;
            Initialiaze();
        }

        public UserControlViewModel(BPMNDBEntities context, EntityObject obj, EntityObject obj2)
        {
            bpmnEty = context;
            Entity = obj;
            Entity2 = obj2;
            Initialiaze();
        }



        protected virtual void Initialiaze() { }

        public BPMNDBEntities bpmnEty { get; set; }

        public EntityObject Entity { get; set; }

        public EntityObject Entity2 { get; set; }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }


        public virtual void Dispose()
        {
            this.bpmnEty.Dispose();
        }
    }
}
