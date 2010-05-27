using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
    public class Skin : Persistente
	{
        public virtual String Path { get; set; }
        public virtual String Description { get; set; }
        public virtual int Skinid { get; set; }
        public virtual int Themeid { get; set; }
        public virtual Theme Theme { get; set; }
		public virtual ISet<ElementSkin> ElementSkins { get; set; }
		public virtual ISet<Page> Pages { get; set; }

        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as Skin);
        }
		
		public virtual bool Equals(Skin obj)
		{
			if (obj == null) return false;

			if (Equals(Skinid, obj.Skinid) == false)
				return false;

			if (Equals(Description, obj.Description) == false)
				return false;

			return true;
		}

        public override int GetHashCode() {
            int result = 1;

            result = (result * 397) ^ (Skinid != null ? Skinid.GetHashCode() : 0);
            result = (result * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            return result;
        }

	}
}