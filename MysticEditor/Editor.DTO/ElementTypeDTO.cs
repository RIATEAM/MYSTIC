using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor.DTO {
    public class ElementTypeDTO : PersistenteDTO {

        public virtual String Description { get; set; }
        public virtual int Elementtypeid { get; set; }
    }
}
