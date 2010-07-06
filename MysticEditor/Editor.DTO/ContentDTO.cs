using System;

namespace Editor.DTO {
    public class ContentDTO : PersistenteDTO {

        public int Contentid { get; set; }
        public int Parentcontentid { get; set; }
        public String Title { get; set; }
        public int Skinid { get; set; }
        public int Themeid { get; set; }
        public int Iditem { get; set; }
        public String Repository { get; set; }
        public String Date_creation { get; set; }
        public String Date_publish { get; set; }
        public int Publish_active { get; set; }
        public int State { get; set; }

        public  bool IsPersisted {
            get {
                return Contentid > 0;
            }
        }

    }
}
