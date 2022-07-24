using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputHandler : IInputHandler
{
    private KeyCode spawnKeyCode;
    private KeyCode despawnKeyCode;

    public DesktopInputHandler(KeyCode spawnKeyCode, KeyCode despawnKeyCode)
    {
        this.spawnKeyCode = spawnKeyCode;
        this.despawnKeyCode = despawnKeyCode;
    }
}