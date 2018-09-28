using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyPartComponent))]
public abstract class Jumpable : Prioritizer
{
    //Jump
    public virtual void OnJumpButtonDown() {
        Debug.Log("jump button down!");
    }

    public virtual void OnJumpButtonUp() {
        Debug.Log("jump button up!");
    }
}
