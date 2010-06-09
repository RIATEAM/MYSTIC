
using System.Xml.Serialization;
namespace Editor.DTO {

    public class PersistenteDTO : IPersistenteDTO {


        #region  Membri di IPersistente

        public bool IsNew {
            get;
            set;
        }
        public bool Dirty {
            get;
            set;
        }
        public bool Deleted {
            get;
            set;
        }
        public bool IsPersisted {
            get;
            set;
        }
        /// <summary>
        /// Proprietà readonly, IsNew || Dirty || Deleted
        /// </summary>
        public bool HasChanged {
            get {
                return IsNew || Dirty || Deleted;
            }

        }
        #endregion
    }
}
