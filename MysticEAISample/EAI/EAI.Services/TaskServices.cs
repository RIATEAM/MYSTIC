using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EAI.BE.Model;
using EAI.BL;
using EAI.BE;

namespace EAI.Services {
    public class TaskServices {
        public List<Task> GetTasks() {
            TaskLogic tskSrv = new TaskLogic();
            return tskSrv.GetTasks();
        }

        public List<Task> GetTasks(string where) {
            TaskLogic tskSrv = new TaskLogic();
            return tskSrv.GetTasks(where);
        }
        
        public static void NewTask(int type, int valore, string codice) {
            Task task = new Task();
            task.Valore = valore;
            task.Type = type;
            task.Codice = codice;
            task.DateInsert = task.DateUpd = DateTime.Now;
            HibernateHelper.InsertCommand(task);
        }

        public bool UpdateStatusTask(Task obj) {

            TaskLogic tskSrv = new TaskLogic();
            Task tsk = tskSrv.UpdateTask(obj);
            if (tsk.Dirty) {
                //Non è stato modificato
                // Loggare errore
                return false;
            } else {
                //E' stato modificato                
                return true;
            }

        }
        
        public bool UpdateStatusTask(int taskid, int status) {
            Task tst = new Task();
            tst = HibernateHelper.SelectIstance<Task>(new string[] { "Id" }, new object[] { taskid }, new Operators[] { Operators.Eq });
            tst.Status = status;
            tst.DateUpd = DateTime.Now;

            TaskLogic tskSrv = new TaskLogic();
            tst = tskSrv.UpdateTask(tst);
            if (tst.Dirty) {
                //Non è stato modificato
                // Loggare errore
                return false;
            } else {
                //E' stato modificato                
                return true;
            }
        }
    }
}
