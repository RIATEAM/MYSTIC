using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.DTO {
    public class PageDTO : PersistenteDTO {

        public int Contentid { get; set; }
        public int Pageid { get; set; }
        public int Parentpageid { get; set; }
        public int Structureid { get; set; }
        public int Position { get; set; }
        public int Level { get; set; }
        public String Title { get; set; }
        public String Publictitle { get; set; }
        public int? Skinid { get; set; }
        public int State { get; set; }

        public List<PageElementDTO> PageelementsList { get; set; }

    
        public override bool IsPersisted {
            get {
                return Pageid > 0;
            }
        }

    }
}
