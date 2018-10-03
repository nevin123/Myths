using UnityEngine;

public class BodyPartComponent : MonoBehaviour
{
    public BodyPart bodyPart;
    public Vector2 pivot;

    void OnDrawGizmos() {
        Gizmos.DrawIcon((Vector2)transform.position + pivot, "pivot", true);
    }
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