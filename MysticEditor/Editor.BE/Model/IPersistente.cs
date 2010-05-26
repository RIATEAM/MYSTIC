using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;


namespace Editor.BE.Model {
    public interface IPersistente {
        /// <summary>
        /// Elemento creato nuovo
        /// </summary>
        bool IsNew { get; set; }
        /// <summary>
        /// Elemento modificato
        /// </summary>
        bool Dirty { get; set; }
        /// <summary>
        /// Elemento eliminato
        /// </summary>
        bool Deleted { get; set; }
        /// <summary>
        /// Elemento persistito
        /// </summary>
        bool IsPersisted { get; set; } 
    }


}
