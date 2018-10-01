using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyPartComponent))]
public abstract class Movable : Prioritizer
{
    //Move
    public virtual void Move(Vector2 input, ref Vector2 velocity) {
        
    }
}
