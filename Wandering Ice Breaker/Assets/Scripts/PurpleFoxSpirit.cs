using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleFoxSpirit : Enemy
{

    private void Awake()
    {
        setTimer(2.0f); // Changing the AI Timer, it should take a lot longer for this spirit to get moving
    }
    override public void Move() // A* Movement 
    {
        return;
    }
}
