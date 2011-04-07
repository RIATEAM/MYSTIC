using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.BE.Model
{
    [Serializable]
    public class View
    {
        public virtual Int32 Codice { get; set; }
        public virtual Int32? Type { get; set; }
    }
}
