using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.DTO {
    public class ElementDTO : PersistenteDTO {
        public String Description { get; set; }
        public int Elementid { get; set; }
        public int Elementtypeid { get; set; }
        public int Structureid { get; set; }


        public override bool IsPersisted {
            get {
                return Elementid > 0;
            }
        }

        

    }
}