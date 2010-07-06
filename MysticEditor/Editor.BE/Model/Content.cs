using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using System.Xml.Serialization;

namespace Editor.BE.Model {
    [Serializable]
    public class Content : Persistente {

        public virtual int Contentid { get; set; }
        public virtual int Parentcontentid { get; set; }
        public virtual String Title { get; set; }
        public virtual int Skinid { get; set; }
        public virtual Skin Skin { get; set; }
        public virtual ISet<Page> Pages { get; set; }
        public virtual ISet<Widget> Widgets { get; set; }
        public virtual int Themeid { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual int Iditem { get; set; }
        public virtual String Repository { get; set; }
        public virtual String Date_creation { get; set; }
        public virtual String Date_publish { get; set; }
        public virtual int Publish_active { get; set; }
        public virtual int State { get; set; }

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as Content);
        }

        public virtual bool Equals(Content obj) {
            if (obj == null) return false;

            if (Equals(Contentid, obj.Contentid) == false)
                return false;

            if (Equals(Parentcontentid, obj.Parentcontentid) == false)
                return false;

            if (Equals(Title, obj.Title) == false)
                return false;

            if (Equals(Skinid, obj.Skinid) == false)
                return false;

            if (Equals(Themeid, obj.Themeid) == false)
                return false;

            if (Equals(Iditem, obj.Iditem) == false)
                return false;

            if (Equals(Repository, obj.Repository) == false)
                return false;

            if (Equals(Date_creation, obj.Date_creation) == false)
                return false;

            if (Equals(Date_publish, obj.Date_publish) == false)
                return false;

            if (Equals(Publish_active, obj.Publish_active) == false)
                return false;

            return true;
        }

        public override int GetHashCode() {
            int result = 1;

            result = (result * 397) ^ (Contentid != null ? Contentid.GetHashCode() : 0);
            result = (result * 397) ^ (Parentcontentid != null ? Parentcontentid.GetHashCode() : 0);
            result = (result * 397) ^ (Title != null ? Title.GetHashCode() : 0);
            result = (result * 397) ^ (Skinid != null ? Skinid.GetHashCode() : 0);
            result = (result * 397) ^ (Themeid != null ? Themeid.GetHashCode() : 0);
            result = (result * 397) ^ (Iditem != null ? Iditem.GetHashCode() : 0);
            result = (result * 397) ^ (Repository != null ? Repository.GetHashCode() : 0);
            result = (result * 397) ^ (Date_creation != null ? Date_creation.GetHashCode() : 0);
            result = (result * 397) ^ (Date_publish != null ? Date_publish.GetHashCode() : 0);
            result = (result * 397) ^ (Publish_active != null ? Publish_active.GetHashCode() : 0);
            return result;
        }

        public override bool IsPersisted {
            get {
                return Contentid > 0;
            }
        }
    }
}