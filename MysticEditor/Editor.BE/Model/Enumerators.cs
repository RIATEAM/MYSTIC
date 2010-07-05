using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor.BE.Model.Enumerators {


    public enum ElementTypeEnum : short {
        LabelText = 1,
        RawHtml = 2,
        Img = 3,
        Link = 4,
        UtilityLink = 5
    }

    public enum PageStateEnum : short {
        Nessuna = 1,
        NonApplicabbile = 2,
        NonCliccabile = 3,
        Aggiornato = 4,
        Nuovo = 5,
        Eliminato = 99
    }

    public enum WidgetElementTypeEnum : short {
        Creato = 1,
        Importato = 2
    }


}
