using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Creature : MonoBehaviour
{   
    public GameObject[] bodyPartsToSpawn;

    Controller2D controller;

    //BodyParts
    List<BodyPartComponent> allBodyParts;
    Body[] bodies;
    Movable[] movables;
    Jumpable[] jumpables;

    public Body body;
    public Movable movable;
    public Jumpable jumpable;

    void Awake() {
        controller = GetComponent<Controller2D>();

        allBodyParts = new List<BodyPartComponent>();

        foreach(GameObject bodyPart in bodyPartsToSpawn) {
            if(bodyPart.GetComponent<BodyPartComponent>() != null) {
                allBodyParts.Add(bodyPart.GetComponent<BodyPartComponent>());
            }
        }

        SpawnBody(allBodyParts.ToArray());
        SpawnBodyParts(allBodyParts.ToArray());

        movables = transform.GetComponentsInChildren<Movable>();
        jumpables = transform.GetComponentsInChildren<Jumpable>();

        SortInteractables();
    }

    void SortInteractables() {
        movables.OrderBy(x => x.priority).ToArray();
        movable = movables[0];
        movable.SetController(controller);

        jumpables.OrderBy(x => x.priority).ToArray();
        jumpable = jumpables[0];
        jumpable.SetController(controller);
    }

    void SpawnBody(BodyPartComponent[] allBodyParts) {
        int highestPriority = int.MinValue;

        foreach(BodyPartComponent part in allBodyParts) {
            if(part.bodyPart == BodyPart.Body) {
                if(part.transform.GetComponent<Body>() != null) {
                    Body body = part.transform.GetComponent<Body>();

                    if(body.priority > highestPriority) {
                        this.body = body;

                        GameObject pivot = new GameObject("pivot");
                        pivot.transform.SetParent(transform, false);
                        
                        GameObject newPart = Instantiate(part.gameObject, -part.pivot, Quaternion.identity);
                        newPart.transform.SetParent(pivot.transform, false);
                    }
                }
            } 
        }
    }

    void SpawnBodyParts(BodyPartComponent[] allBodyParts) {
        int legIndex = 0;

        foreach(BodyPartComponent part in allBodyParts) {
            //You can only have one body and the body is already spawned
            if(part.bodyPart == BodyPart.Body) {
                continue;
            }

            Vector2 pivotPosition;

            int numberOfParts = body.NumberOfParts(part.bodyPart);
            if(numberOfParts >= legIndex + 1) {
                pivotPosition = body.GetBodyPartPosition(part.bodyPart, 0);
            } else {
                continue;
            }

            GameObject pivot = new GameObject("pivot");
            pivot.transform.SetParent(transform, false);
            pivot.transform.localPosition = pivotPosition;
            
            GameObject newPart = Instantiate(part.gameObject, -part.pivot, Quaternion.identity);
            newPart.transform.SetParent(pivot.transform, false);

            //Add to index
            switch(part.bodyPart) {
                case BodyPart.Legs:
                    legIndex++;
                break;
            }
        }
    }

    // void PositionBodyParts(BodyPartComponent[] allBodyParts) {
    //     int legIndex = 0;

    //     foreach(BodyPartComponent part in allBodyParts) {
    //         Vector2 pivotPosition;

    //         int numberOfParts = body.NumberOfParts(part.bodyPart);
    //         if(numberOfParts > 0) {
    //             pivotPosition = body.GetBodyPartPosition(part.bodyPart, 0);
    //         } else {
    //             return;
    //         }
    //         pivot.transform.position = pivotPosition;
    //     }
    // }
}