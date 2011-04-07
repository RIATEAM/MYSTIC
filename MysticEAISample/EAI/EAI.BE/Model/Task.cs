using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.BE.Model
{
    [Serializable]
    public class Task : Persistente
    {
        public virtual Int32 ID { get; set; }
        public virtual Int32? Valore { get; set; }
        public virtual String Codice { get; set; }
        public virtual Int32? Type { get; set; }
        public virtual Int32? Status { get; set; }
        public virtual DateTime DateInsert { get; set; }
        public virtual DateTime DateUpd { get; set; }
    }
}
