# TestUnity
-Write a pooling system :
    -Should be as generic as possible
    -Allows you to choose the amount of objects to put in the pool by default per object in the pool.
    -Should allow expansion of the pool.
 
-Write a script "FindNearestNeighbour" that will automatically find the nearest GameObject that also have this script attached to it :
    -This script should prioritize execution time (be as optimized as possible)
    -If a new GameObject is instantiated with this script attached it should be taken in account by the other gameobjects.
    -Display the closest gameobject by drawing a line in between the two gameobjects.
 
-Write a system making gameobjects with a specific script attached to them move randomly (3D space) within a zone that can be defined by three floats x,y,z exposed to the editor.
    -This system should prioritize execution time (be as optimized as possible).
 
-Create an example scene implementing the previous three tasks :
    -Create a prefab with a cube GameObject (Just a mesh renderer, no need for any kind of physics or collider), add the FindNearestNeighbour script + the random movement script
    -Should use the pooling system to spawn/despawn the previously created prefab.
    -Create a simple UI displaying the current amount of spawned prefabs + an input field.
    -Create a script that spawn x amount of the prefab on start and lets you spawn and despawn x prefabs (x gets its value from the input field) with keys that are exposed to the editor. The spawned prefab should be spawned at a random position within the zone created by the last task system.
 
