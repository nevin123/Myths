using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyPartComponent))]
public abstract class Jumpable : Prioritizer
{
    [HideInInspector]
    public Controller2D controller;

    public void SetController(Controller2D controller) {
        this.controller = controller;
    }

    //Move
    public virtual void Move(ref Vector2 velocity) {
        Debug.Log("JumpMove");
    }

    //Jump
    public virtual void OnJumpButtonDown(ref Vector2 velocity, Vector2 input) {
        Debug.Log("jump button down!");
    }

    public virtual void OnJumpButtonUp(ref Vector2 velocity) {
        Debug.Log("jump button up!");
    }
}
