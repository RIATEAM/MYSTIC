using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using System.Linq;

namespace Editor.BE.Model {
    [Serializable]
    public class Widget : Persistente {

        public virtual int Widgetid { get; set; }
        public virtual int Contentid { get; set; }
        public virtual String Title { get; set; }
        public virtual String Publictitle { get; set; }
        public virtual int Skinid { get; set; }
        public virtual int Position { get; set; }
        public virtual int Structureid { get; set; }
        public virtual int State { get; set; }
        public virtual Content Content { get; set; }
        public virtual Skin Skin { get; set; }
        public virtual ISet<WidgetElement> WidgetElements { get; set; }


        private List<WidgetElement> _WidgetElementsList;

        public virtual List<WidgetElement> WidgetElementsList {
            get {
                if (WidgetElements != null) {
                    _WidgetElementsList = WidgetElements.ToList<WidgetElement>();
                    _WidgetElementsList.Sort(delegate(WidgetElement w1, WidgetElement w2) { return w1.Position.CompareTo(w2.Position); });
                }

                return _WidgetElementsList;
            }
            set { _WidgetElementsList = value; }
        }


        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as Page);
        }

        public virtual bool Equals(Page obj) {
            if (obj == null) return false;

            if (Equals(Widgetid, obj.Pageid) == false)
                return false;

            if (Equals(Contentid, obj.Contentid) == false)
                return false;

            if (Equals(Skinid, obj.Skinid) == false)
                return false;

            if (Equals(Title, obj.Title) == false)
                return false;

            if (Equals(Publictitle, obj.Publictitle) == false)
                return false;

            if (Equals(State, obj.State) == false)
                return false;


            return true;
        }

        public override int GetHashCode() {
            int result = 1;

            result = (result * 397) ^ (Widgetid != null ? Widgetid.GetHashCode() : 0);
            result = (result * 397) ^ (Contentid != null ? Contentid.GetHashCode() : 0);
            result = (result * 397) ^ (Skinid != null ? Skinid.GetHashCode() : 0);
            result = (result * 397) ^ (Title != null ? Title.GetHashCode() : 0);
            result = (result * 397) ^ (Publictitle != null ? Publictitle.GetHashCode() : 0);
            result = (result * 397) ^ (State != null ? State.GetHashCode() : 0);
            return result;
        }
        public override bool IsPersisted {
            get {
                return Widgetid > 0;
            }
        }

    }
}
