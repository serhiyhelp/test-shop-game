using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitState : GameState
{
    public override bool MovementAllowed { get => false; }
}
