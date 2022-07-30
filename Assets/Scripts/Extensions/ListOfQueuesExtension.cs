using System.Collections.Generic;
using System.Linq;

public static class ListOfQueuesExtension
{
    public static List<T> GetAllElementsAsList<T>(this List<Queue<T>> listOfQueues)
    {
        List<T> allObjects = new List<T>();

        foreach (Queue<T> queue in listOfQueues)
        {
            List<T> poolableObjects = queue.ToList();

            foreach (var poolableObject in poolableObjects)
            {
                allObjects.Add(poolableObject);
            }
        }

        return allObjects;
    }
}