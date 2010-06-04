using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor.DTO {
    public class ContentDTO : PersistenteDTO {

        public int Contentid { get; set; }
        public int Parentcontentid { get; set; }
        public String Title { get; set; }
        public int Skinid { get; set; }

        public override bool IsPersisted {
            get {
                return Contentid > 0;
            }
        }

    }
}
