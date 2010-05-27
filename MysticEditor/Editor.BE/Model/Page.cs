using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace Editor.BE.Model {
    [Serializable]
    public class Page : Persistente {

        public virtual int Contentid { get; set; }
        public virtual int Pageid { get; set; }
        public virtual int Parentpageid { get; set; }
        public virtual int Structureid { get; set; }
        public virtual int Position { get; set; }
        public virtual int Level { get; set; }
        public virtual String Title { get; set; }
        public virtual String Publictitle { get; set; }
        public virtual int? Skinid { get; set; }
        public virtual int State { get; set; }

        public virtual Content Content { get; set; }
        public virtual Skin Skin { get; set; }
        public virtual ISet<PageElement> PageElements { get; set; }

        private IList<PageElement> _PageElementsList;

        public virtual IList<PageElement> PageelementsList {
            get {
                if (PageElements != null) {
                    _PageElementsList= PageElements.ToList<PageElement>();
                }             
                
                return _PageElementsList;             
            }  
            set { _PageElementsList = value; }
        }


        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as Page);
        }

        public virtual bool Equals(Page obj) {
            if (obj == null) return false;

            if (Equals(Pageid, obj.Pageid) == false)
                return false;

            if (Equals(Parentpageid, obj.Parentpageid) == false)
                return false;

            if (Equals(Contentid, obj.Contentid) == false)
                return false;

            if (Equals(Skinid, obj.Skinid) == false)
                return false;

            if (Equals(Title, obj.Title) == false)
                return false;

            if (Equals(Publictitle, obj.Publictitle) == false)
                return false;

            if (Equals(Position, obj.Position) == false)
                return false;

            if (Equals(State, obj.State) == false)
                return false;

            if (Equals(Structureid, obj.Structureid) == false)
                return false;


            return true;
        }

        public override int GetHashCode() {
            int result = 1;

            result = (result * 397) ^ (Pageid != null ? Pageid.GetHashCode() : 0);
            result = (result * 397) ^ (Parentpageid != null ? Parentpageid.GetHashCode() : 0);
            result = (result * 397) ^ (Contentid != null ? Contentid.GetHashCode() : 0);
            result = (result * 397) ^ (Skinid != null ? Skinid.GetHashCode() : 0);
            result = (result * 397) ^ (Title != null ? Title.GetHashCode() : 0);
            result = (result * 397) ^ (Publictitle != null ? Publictitle.GetHashCode() : 0);
            result = (result * 397) ^ (Position != null ? Position.GetHashCode() : 0);
            result = (result * 397) ^ (Level != null ? Level.GetHashCode() : 0);
            result = (result * 397) ^ (State != null ? State.GetHashCode() : 0);
            result = (result * 397) ^ (Structureid != null ? Structureid.GetHashCode() : 0);
            return result;
        }
        
        public override bool IsPersisted {
            get {
                return Pageid > 0;
            }
        }

    }
}