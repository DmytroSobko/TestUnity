using System.Collections.Generic;
using System.Linq;

public static class ListOfQueuesExtension
{
    public static List<T> GetAllElementsAsList<T, T1>(this List<Queue<T1>> listOfQueues) where T1 : T
    {
        List<T> allObjects = new List<T>();

        foreach (Queue<T1> queue in listOfQueues)
        {
            List<T1> poolableObjects = queue.ToList();

            foreach (var poolableObject in poolableObjects)
            {
                allObjects.Add(poolableObject);
            }
        }

        return allObjects;
    }
}
