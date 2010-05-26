using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor.DTO {
    public class PageElementDTO :PersistenteDTO{

        public  int Elementid { get; set; }
        public  int PageElementid { get; set; }
        public  int Pageid { get; set; }
        public  String Value { get; set; }
        public  String Filename { get; set; }

        public override bool IsPersisted {
            get {
                return PageElementid > 0;
            }
        }
    }
}
