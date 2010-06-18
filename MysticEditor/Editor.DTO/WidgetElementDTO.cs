using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Editor.DTO {

    public class WidgetElementDTO : PersistenteDTO {

        public int Widgetelementid { get; set; }
        public int Elementid { get; set; }
        public int Widgetid { get; set; }
        public String Valore { get; set; }
        public String Name { get; set; }
        public int Position { get; set; }
        public ElementDTO Element { get; set; }

        public bool IsPersisted {
            get {
                return Widgetelementid > 0;
            }
        }
    }
}
