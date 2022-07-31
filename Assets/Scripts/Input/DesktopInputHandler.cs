using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DesktopInputHandler: IInputHandler<KeyCode>
{
    public event Action<KeyCode> InputTookPlace;

    private List<KeyCode> keyCodes;

    public DesktopInputHandler(List<KeyCode> keyCodes)
    {
        this.keyCodes = keyCodes;
    }

    public void Process()
    {
        if (keyCodes.Any(CheckAnyPressed))
        {
            var keyCode = keyCodes.First(CheckAnyPressed);
            InputTookPlace?.Invoke(keyCode);
        }
    }

    private bool CheckAnyPressed(KeyCode keyCode)
        => Input.GetKeyDown(keyCode);
}