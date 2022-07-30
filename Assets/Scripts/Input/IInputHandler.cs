using System;

public interface IInputHandler<TPayload> : IUpdatable, IService
{
    public event Action<TPayload> InputTookPlace;
}