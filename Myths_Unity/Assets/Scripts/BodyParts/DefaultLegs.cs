using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegs : Body, IMovable
{
    //Is Bipedal
    [SerializeField]
    private bool isBipedal;
    public bool bipedal 
    {
        get {return isBipedal;}
        set {isBipedal = value;}
    }

    public void Move(float direction) {
        transform.parent.Translate(direction * 5 * Time.deltaTime,0,0);
    }
}
