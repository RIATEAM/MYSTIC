using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.BE.Model
{
    class EaiTask : Persistente
    {
        public virtual Int32 TaskID { get; set; }
        public virtual Int32 MessageID { get; set; }
        public virtual Int32 UserID { get; set; }
        public virtual Int32 Type { get; set; }
        public virtual Int32 Status { get; set; }
    }
}
