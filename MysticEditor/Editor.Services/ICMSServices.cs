using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Editor.Services {
    public interface ICMSServices {
        string GetItemPath(string iditemamm, string type);
        string GetItemTitle(int iditemamm, string type);
        string GetItemIdUser(int iditemamm);
    }
}
