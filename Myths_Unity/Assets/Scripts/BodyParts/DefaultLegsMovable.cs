using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegsMovable : Movable
{
    void Start() {
        Move(Vector2.zero);
    }

    public override void Move(Vector2 input) {
        Debug.Log("Moving Default Legs");
    }
}