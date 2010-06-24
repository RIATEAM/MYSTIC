using System;

namespace Editor.DTO {
    public class ThemeDTO : PersistenteDTO {

        public virtual String Path { get; set; }
        public virtual String Description { get; set; }
        public virtual int Themeid { get; set; }
        public virtual int Templateid { get; set; }
    }
}
