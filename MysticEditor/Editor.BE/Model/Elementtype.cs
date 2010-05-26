using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class ElementType : Persistente
	{
		public virtual String Description { get; set; }
		public virtual int Elementtypeid { get; set; }
		public virtual ISet<Element> Elements { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
				
			return Equals(obj as ElementType);
		}
		
		public virtual bool Equals(ElementType obj)
		{
			if (obj == null) return false;

			if (Equals(Elementtypeid, obj.Elementtypeid) == false)
				return false;

			if (Equals(Description, obj.Description) == false)
				return false;

			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

			result = (result * 397) ^ (Elementtypeid != null ? Elementtypeid.GetHashCode() : 0);
			result = (result * 397) ^ (Description != null ? Description.GetHashCode() : 0);			
			return result;
		}

	}
}