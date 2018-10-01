using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BodyPartComponent))]
public abstract class Body : Prioritizer
{
    public Transform headPosition;
}
