using System.Collections.Generic;

public class UpdatableService : IService
{
    public List<IUpdatable> updatables = new List<IUpdatable>();

    public void AddUpdatable(IUpdatable updatable)
    {
        updatables.Add(updatable);
    }

    public void RemoveUpdatable(IUpdatable updatable)
    {
        updatables.Remove(updatable);
    }

    public void Update()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].Process();
        }
    }
}