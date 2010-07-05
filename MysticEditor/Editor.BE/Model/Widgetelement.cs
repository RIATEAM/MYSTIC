using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class WidgetElement : Persistente
	{
		public virtual int Widgetelementid { get; set; }
        public virtual int Elementid { get; set; }
        public virtual int Widgetid { get; set; }
		public virtual String Valore { get; set; }
        public virtual String Name { get; set; }
        public virtual int Position { get; set; }
        public virtual int Type { get; set; }

        public virtual Element Element { get; set; }
        public virtual Widget Widget { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;

            return Equals(obj as WidgetElement);
		}
		
		public virtual bool Equals(WidgetElement obj)
		{
			if (obj == null) return false;

            if (Equals(Widgetelementid, obj.Widgetelementid) == false)
				return false;

            if (Equals(Widgetid, obj.Widgetid) == false)
				return false;

			if (Equals(Elementid, obj.Elementid) == false)
				return false;

            if (Equals(Valore, obj.Valore) == false)
				return false;

			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

            result = (result * 397) ^ (Widgetelementid != null ? Widgetelementid.GetHashCode() : 0);
            result = (result * 397) ^ (Widgetid != null ? Widgetid.GetHashCode() : 0);
			result = (result * 397) ^ (Elementid != null ? Elementid.GetHashCode() : 0);
            result = (result * 397) ^ (Valore != null ? Valore.GetHashCode() : 0);			
			return result;
		}
        public override bool IsPersisted {
            get {
                return Widgetelementid > 0;
            }
        }

	}
}