using System;

namespace Editor.DTO {
    public class ContentDTO : PersistenteDTO {

        public int Contentid { get; set; }
        public int Parentcontentid { get; set; }
        public String Title { get; set; }
        public int Skinid { get; set; }

        public  bool IsPersisted {
            get {
                return Contentid > 0;
            }
        }

    }
}
