using System;
using System.Collections.Generic;
using Akka.Actor;

namespace AssistCore.Human
{
    public class Task : IEquatable<Task>, IComparable<Task>
    {
        public readonly Guid Id = Guid.NewGuid();
        public readonly string Name;
        public readonly double Priority;
        public readonly IActorRef Owner;

        public Task(string name, double priority, IActorRef owner)
        {
            Name = name;
            Priority = priority;
            Owner = owner;
        }

        public int CompareTo(Task other)
        {
            var comp = Priority.CompareTo(other.Priority);
            if (comp == 0)
            {
                comp = Id.CompareTo(other.Id);
            }
            return comp;
        }

        public bool Equals(Task other)
        {
            if (other is null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object other)
        {
            if (other is Task t)
            {
                return Equals(t);
            }
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(Task a, Task b)
        {

            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (Object.ReferenceEquals(null, a))
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Task a, Task b)
        {
            return !(a == b);
        }
    }

    public class UpdateTask
    {
        public readonly Task From;
        public readonly Task To;

        public UpdateTask(Task from, Task to)
        {
            From = from;
            To = to;
        }
    }

    public class CancelTask
    {
        public readonly Task Task;

        public CancelTask(Task task)
        {
            Task = task;
        }
    }

    public class TaskCompleted
    {
        public readonly Task Target;
        public readonly DateTime When;

        public TaskCompleted(Task target, DateTime when)
        {
            Target = target;
            When = when;
        }
    }

    public class TaskRejected
    {
        public readonly Task Target;
        public readonly DateTime When;

        public TaskRejected(Task target, DateTime when)
        {
            Target = target;
            When = when;
        }
    }
}
