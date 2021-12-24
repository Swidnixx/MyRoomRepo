using System;


    class Command : ICommand
    {
        public Command(string title, string msg, Func<string> command)
        {
            this.title = title;
            this.msg = msg;
            this.command = command;
        }

        public string title { get; private set; }
        public string msg { get; private set; }
        public Func<string> command { get; private set; }
    }
