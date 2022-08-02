using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DesktopInputHandler : InputHandler<KeyCode>
{
    public DesktopInputHandler(List<KeyCode> keyCodes) : base(keyCodes)
    {

    }

    protected override KeyCode CheckTrigger()
    {
        return triggers.FirstOrDefault(keyCode => Input.GetKeyDown(keyCode));
    }
}