using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class Structure : Persistente
	{
		public virtual String Description { get; set; }
		public virtual int Structureid { get; set; }
		public virtual ISet<Element> Elements { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
				
			return Equals(obj as Structure);
		}
		
		public virtual bool Equals(Structure obj)
		{
			if (obj == null) return false;

			if (Equals(Structureid, obj.Structureid) == false)
				return false;

			if (Equals(Description, obj.Description) == false)
				return false;

			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

			result = (result * 397) ^ (Structureid != null ? Structureid.GetHashCode() : 0);
			result = (result * 397) ^ (Description != null ? Description.GetHashCode() : 0);			
			return result;
		}

	}
}