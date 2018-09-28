using UnityEngine;

public class BodyPartComponent : MonoBehaviour
{
    public BodyPart bodyPart;
}

//Enum
public enum BodyPart {
    Body,
    Head,
    Legs,
    Arms,
    Tail,
    Wings
}