using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Editor.DTO {

    public class WidgetDTO : PersistenteDTO {

        public int Widgetid { get; set; }
        public int Contentid { get; set; }
        public String Title { get; set; }
        public String Publictitle { get; set; }
        public int Skinid { get; set; }
        public int State { get; set; }
        public int Position { get; set; }
        public int Structureid { get; set; }

        public WidgetElementDTO[] WidgetElementsList { get; set; }

       
        public bool IsPersisted {
            get {
                return Widgetid > 0;
            }
        }

    }
}
