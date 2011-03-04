using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Editor.Services;
using System.Data;

namespace Editor.Scheduler {
    public class CMSCustomer {

        private ICMSServices _CMSServices;
        [Dependency]
        public ICMSServices CMSServices {
            get { return _CMSServices; }
            set { _CMSServices = value; }
        }
        public string GetItemPath(string iditemamm, string type) {
            return CMSServices.GetItemPath(iditemamm, type);
        }
        public string GetItemTitle(int iditemamm, string type) {
            return CMSServices.GetItemTitle(iditemamm, type);
        }
        public string GetItemIdUser(int iditemamm) {
            return CMSServices.GetItemIdUser(iditemamm);
        }
        public DataSet GetCorrelated(int iditemamm, string type) {
            return CMSServices.GetCorrelated(iditemamm, type);
        }
        public void IndexItem(int iditemuser) {
            CMSServices.IndexItem(iditemuser);
        }
        public void UpdStdDoc(int iditemuser) { 
            CMSServices.UpdStdDoc(iditemuser); 
        }
        public void UpdCommDoc(int iditemamm) { 
            CMSServices.UpdCommDoc(iditemamm); 
        }

    }
}
