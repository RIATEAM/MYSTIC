using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class PageElement : Persistente
	{
		public virtual int Elementid { get; set; }
		public virtual int PageElementid { get; set; }
		public virtual int Pageid { get; set; }
		public virtual String Value { get; set; }
        public virtual String Filename { get; set; }

        public virtual Element Element { get; set; }        
        public virtual Page Page { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;

            return Equals(obj as PageElement);
		}

        public virtual bool Equals(PageElement obj)
		{
			if (obj == null) return false;

			if (Equals(PageElementid, obj.PageElementid) == false)
				return false;

			if (Equals(Pageid, obj.Pageid) == false)
				return false;

			if (Equals(Elementid, obj.Elementid) == false)
				return false;

			if (Equals(Value, obj.Value) == false)
				return false;

			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

			result = (result * 397) ^ (PageElementid != null ? PageElementid.GetHashCode() : 0);
			result = (result * 397) ^ (Pageid != null ? Pageid.GetHashCode() : 0);
			result = (result * 397) ^ (Elementid != null ? Elementid.GetHashCode() : 0);
			result = (result * 397) ^ (Value != null ? Value.GetHashCode() : 0);			
			return result;
		}

	}
}