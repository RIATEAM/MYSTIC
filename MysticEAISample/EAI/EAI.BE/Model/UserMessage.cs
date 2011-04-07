using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.BE.Model
{
    [Serializable]
    public class UserMessage : Persistente
    {
        public virtual Int32 UserMessageID { get; set; }
        public virtual Int32? MessageID { get; set; }
        public virtual Int32? UserID { get; set; }
    }
}
