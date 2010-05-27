﻿
using System.Xml.Serialization;
namespace Editor.DTO{
    
    public class PersistenteDTO : IPersistenteDTO {

        
        #region  Membri di IPersistente

        public virtual bool IsNew {
            get;
            set;
        }
        public virtual bool Dirty {
            get;
            set;
        }
        public virtual bool Deleted {
            get;
            set;
        }
        public virtual bool IsPersisted {
            get;
            set;
        }        
        /// <summary>
        /// Proprietà readonly, IsNew || Dirty || Deleted
        /// </summary>
        public virtual bool HasChanged {
            get {
                return IsNew || Dirty || Deleted;
            }

        }
        #endregion
    }
}