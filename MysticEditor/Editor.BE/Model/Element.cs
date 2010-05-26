using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class Element : Persistente
	{
		public virtual String Description { get; set; }
		public virtual int Elementid { get; set; }
		public virtual int Elementtypeid { get; set; }
		public virtual int Structureid { get; set; }
		public virtual ISet<ElementSkin> ElementSkins { get; set; }
        public virtual ElementType ElementType { get; set; }
        public virtual ISet<WidgetElement> PageElements { get; set; }
        public virtual Structure Structure { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
				
			return Equals(obj as Element);
		}
		
		public virtual bool Equals(Element obj)
		{
			if (obj == null) return false;

			if (Equals(Elementid, obj.Elementid) == false)
				return false;

			if (Equals(Structureid, obj.Structureid) == false)
				return false;

			if (Equals(Elementtypeid, obj.Elementtypeid) == false)
				return false;

			if (Equals(Description, obj.Description) == false)
				return false;

			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

			result = (result * 397) ^ (Elementid != null ? Elementid.GetHashCode() : 0);
			result = (result * 397) ^ (Structureid != null ? Structureid.GetHashCode() : 0);
			result = (result * 397) ^ (Elementtypeid != null ? Elementtypeid.GetHashCode() : 0);
			result = (result * 397) ^ (Description != null ? Description.GetHashCode() : 0);			
			return result;
		}

	}
}