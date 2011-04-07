using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.BE.Model
{
    [Serializable]
    public class Message : Persistente
    {
        public virtual Int32? MessageID { get; set; }
        public virtual String Title { get; set; }
        public virtual String Body { get; set; }
        public virtual DateTime DateStart { get; set; }
        public virtual DateTime DateFinish { get; set; }
        public virtual Int32? Type { get; set; }
    }
}
