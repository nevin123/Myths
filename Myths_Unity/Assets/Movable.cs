using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyPartComponent))]
public abstract class Movable : Prioritizer
{
    [HideInInspector]
    public Controller2D controller;

    // Controller2D controller;

    public virtual void SetController(Controller2D controller) {
        this.controller = controller;
    }

    //Move
    public virtual void Move(ref Vector2 velocity, Vector2 input) {
        
    }
}
