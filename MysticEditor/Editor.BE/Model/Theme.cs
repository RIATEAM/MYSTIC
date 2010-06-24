using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Editor.BE.Model
{
	[Serializable]
	public class Theme: Persistente
	{
		public virtual String Path { get; set; }
        public virtual String Description { get; set; }
        public virtual int Themeid { get; set; }
        public virtual ISet<Skin> Skins { get; set; }
        public virtual int Templateid { get; set; }
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
				
			return Equals(obj as Theme);
		}
		
		public virtual bool Equals(Theme obj)
		{
			if (obj == null) return false;

			if (Equals(Themeid, obj.Themeid) == false)
				return false;

			if (Equals(Path, obj.Path) == false)
				return false;

			if (Equals(Description, obj.Description) == false)
				return false;

            if (Equals(Templateid, obj.Templateid) == false)
                return false;

			return true;
		}
		
		public override int GetHashCode()
		{
			int result = 1;

			result = (result * 397) ^ (Themeid != null ? Themeid.GetHashCode() : 0);
			result = (result * 397) ^ (Path != null ? Path.GetHashCode() : 0);
            result = (result * 397) ^ (Description != null ? Description.GetHashCode() : 0);
            result = (result * 397) ^ (Templateid != null ? Templateid.GetHashCode() : 0);		
			return result;
		}
        public override bool IsPersisted {
            get {
                return Themeid > 0;
            }
        }
	}
}