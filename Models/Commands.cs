using SharpHook.Native;
using System;
using System.Collections.Generic;

namespace Jido.Models
{
    public abstract class LowLevelCommand
    {
    }

    public class HighLevelCommand
    {
        public int IntervalInMs { get; set; }
        public bool IsComposite => CompositeCommand != null;
        public LowLevelCommand? Command { get; set; }
        public CompositeCommand? CompositeCommand { get; set; }

        public HighLevelCommand(LowLevelCommand command, int interval)
        {
            if (interval == 0)
            {
                throw new ArgumentException("Interval cannot be 0");
            }
            Command = command;
            IntervalInMs = interval;
        }
    }

    public class ConstantCommand
    {
        public KeyCode KeyToPress { get; set; }
    }

    public class PressCommand : LowLevelCommand
    {
        public KeyCode KeyToPress { get; set; }
        public int PressDurationInMs { get; set; } = 100;
    }

    public class WaitCommand : LowLevelCommand
    {
        public int WaitTimeInMs { get; set; }
    }

    public class CompositeCommand
    {
        public List<LowLevelCommand> Commands { get; set; }
    }
}
