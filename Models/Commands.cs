using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;
using SharpHook.Native;

namespace Jido.Models
{
    public abstract class LowLevelCommand
    { }

    public class PressCommand : LowLevelCommand
    {
        public KeyCode KeyToPress { get; set; }
        public int PressDurationInMs { get; set; } = 100;
    }

    public class WaitCommand : LowLevelCommand
    {
        public int WaitTimeInMs { get; set; }
    }

    public class HighLevelCommand
    {
        public int IntervalInMs { get; set; }
        public bool IsComposite => CommandGroup != null;
        public LowLevelCommand? Command { get; set; }
        public CommandGroup? CommandGroup { get; set; }

        private Timer Timer { get; set; }
        private ConcurrentQueue<LowLevelCommand> CommandQueue { get; set; }

        private void TimerCallback(Object? source, ElapsedEventArgs e)
        {
            if (CommandQueue == null)
                return;
            if (Command != null)
            {
                CommandQueue.Enqueue(Command);
            }
            else if (CommandGroup != null)
            {
                foreach (var command in CommandGroup.Commands)
                {
                    CommandQueue.Enqueue(command);
                }
            }
            Random rnd = new Random();
            Timer.Interval = IntervalInMs * rnd.Next(9, 11) / 10;
        }

        public HighLevelCommand(LowLevelCommand command, int interval)
        {
            if (interval == 0)
            {
                throw new ArgumentException("Interval cannot be 0");
            }
            Command = command;
            IntervalInMs = interval;
            Timer = new Timer(IntervalInMs);
            Timer.Elapsed += TimerCallback;
            Timer.AutoReset = true;
        }

        public void Start(ConcurrentQueue<LowLevelCommand> queue)
        {
            CommandQueue = queue;
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }
    }

    public class CommandGroup
    {
        public List<LowLevelCommand> Commands { get; set; }
    }

    public class ConstantCommand
    {
        public KeyCode KeyToPress { get; set; }
    }
}
