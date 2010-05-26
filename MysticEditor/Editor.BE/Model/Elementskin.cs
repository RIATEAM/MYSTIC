using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class ElementSkin : Persistente
	{
		public virtual int Elementid { get; set; }
		public virtual int ElementSkinid { get; set; }
		public virtual int Skinid { get; set; }
		public virtual Element Element { get; set; }
		public virtual Skin Skin { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
				
			return Equals(obj as ElementSkin);
		}
		
		public virtual bool Equals(ElementSkin obj)
		{
			if (obj == null) return false;

			if (Equals(ElementSkinid, obj.ElementSkinid) == false)
				return false;

			if (Equals(Skinid, obj.Skinid) == false)
				return false;

			if (Equals(Elementid, obj.Elementid) == false)
				return false;

                      
			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

			result = (result * 397) ^ (ElementSkinid != null ? ElementSkinid.GetHashCode() : 0);
			result = (result * 397) ^ (Skinid != null ? Skinid.GetHashCode() : 0);
			result = (result * 397) ^ (Elementid != null ? Elementid.GetHashCode() : 0);
            return result;
		}

	}
}