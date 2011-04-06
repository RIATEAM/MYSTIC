using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.BE.Model
{
    public class Users: Persistente
    {
        public virtual Int32 UserID { get; set; }
        public virtual String Nome { get; set; }
        public virtual String Cognome { get; set; }
        public virtual String Telefono { get; set; }
        public virtual String EmailPrivate { get; set; }
        public virtual String EmailPubbliche { get; set; }



    }
}
