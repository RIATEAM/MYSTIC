using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor.BE.Model {
    [Serializable]
    public class RawHtml : Persistente {

        public virtual int Rawhtmlid { get; set; }
        public virtual String Value { get; set; }

        public override bool IsPersisted {
            get {
                return Rawhtmlid > 0;
            }
        }
    
    }
}
