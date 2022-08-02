using System;
using System.Collections.Generic;

public abstract class InputHandler<T> : IUpdatable, IService
{
    public event Action<T> InputTookPlace;

    protected List<T> triggers;

    public InputHandler(List<T> triggers)
    {
        this.triggers = triggers;
    }

    public void Process()
    {
        T trigger = CheckTrigger();

        InputTookPlace?.Invoke(trigger);
    }

    protected abstract T CheckTrigger();
}

