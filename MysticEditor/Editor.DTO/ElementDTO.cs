using System;

namespace Editor.DTO {
    public class ElementDTO : PersistenteDTO {
        public String Description { get; set; }
        public int Elementid { get; set; }
        public int Elementtypeid { get; set; }
        public int Structureid { get; set; }


        public   bool IsPersisted {
            get {
                return Elementid > 0;
            }
        }

        

    }
}