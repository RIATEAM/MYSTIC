using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Editor.Services {
    public interface ICMSServices {
        string GetItemPath(string iditemamm, string type);
        string GetItemTitle(int iditemamm, string type);
        string GetItemIdUser(int iditemamm);
        DataSet GetCorrelated(int iditemamm, string type);
    }
}
