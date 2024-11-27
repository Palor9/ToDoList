using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    interface Crud // нахуй не надо
    {
        public void CreateTask(string hz);
        public string ReadTask(int ID);
    }
}
