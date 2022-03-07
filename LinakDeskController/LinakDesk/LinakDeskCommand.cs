using System;
using System.Threading.Tasks;

namespace LinakDeskController
{
    public class LinakDeskCommand<T>
    {
        public  Task<T> command { get; private set; }
        public  Action<T> resultAction { get; private set; }

        public LinakDeskCommand(Task<T> command, Action<T> resultAction)
        {
            this.command = command;
            this.resultAction = resultAction;
        }

    }
}
