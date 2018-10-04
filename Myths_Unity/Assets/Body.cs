using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BodyPartComponent))]
public abstract class Body : Prioritizer
{
    public Vector2 headPosition;
    public BodyPartPosition[] bodyPartPositions;

    public virtual int NumberOfParts(BodyPart bodyPart) {
        int numberOfParts = 0;

        foreach(BodyPartPosition bodyPartPosition in bodyPartPositions) {
            if(bodyPartPosition.bodyPart == bodyPart) {
                numberOfParts++;
            }
        }

        return numberOfParts;
    }

    public virtual Vector2 GetBodyPartPosition(BodyPart bodyPart, int index) {
        int i = 0;

        foreach(BodyPartPosition bodyPartPosition in bodyPartPositions) {
            if(bodyPartPosition.bodyPart == bodyPart) {
                i += 1;
                if(i == index) {
                    return bodyPartPosition.position;
                }
            }
        }

        return Vector2.zero;
    }

    public virtual void OnDrawGizmos() {
        Gizmos.DrawIcon((Vector2)transform.position + headPosition, "Head", true);

        foreach(BodyPartPosition bodypartPosition in bodyPartPositions) {
            bodypartPosition.name = bodypartPosition.bodyPart.ToString() + "_" + 1;

            switch(bodypartPosition.bodyPart) {
                case BodyPart.Legs:
                    Gizmos.DrawIcon((Vector2)transform.position + bodypartPosition.position, "Leg", true);
                break;
                case BodyPart.Arms:
                    Gizmos.DrawIcon((Vector2)transform.position + bodypartPosition.position, "Arm", true);
                break;
                case BodyPart.Tail:
                    Gizmos.DrawIcon((Vector2)transform.position + bodypartPosition.position, "Tail", true);
                break;
                case BodyPart.Wings:
                    Gizmos.DrawIcon((Vector2)transform.position + bodypartPosition.position, "Wing", true);
                break;
            }
        }
    }
}

[System.Serializable]
public class BodyPartPosition {
    [HideInInspector]
    public string name;
    public BodyPart bodyPart;
    public Vector2 position;
}
