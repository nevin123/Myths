using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{   
    public GameObject[] bodyPartsToSpawn;

    Controller2D controller;

    //BodyParts
    public BodyPartComponent[] allBodyParts;
    Body[] body;
    Movable[] movables;
    Jumpable[] jumpables;

    public Movable movable;
    public Jumpable jumpable;

    void Awake() {
        controller = GetComponent<Controller2D>();

        foreach(GameObject bodyPart in bodyPartsToSpawn) {
            if(bodyPart.GetComponent<BodyPartComponent>() != null) {
                SpawnBodyPart(bodyPart.GetComponent<BodyPartComponent>());
                // Instantiate(bodyPart, transform);
            }
        }

        allBodyParts = transform.GetComponentsInChildren<BodyPartComponent>();
        movables = transform.GetComponentsInChildren<Movable>();
        jumpables = transform.GetComponentsInChildren<Jumpable>();
        body = transform.GetComponentsInChildren<Body>();

        //Set Movable
        int highestPriority = 0;
        foreach(Movable movable in movables) {
            if(movable.priority > highestPriority) {
                movable.SetController(controller);

                highestPriority = movable.priority;
                this.movable = movable;
            }
        }

        //Set Jumpable
        highestPriority = 0;
        foreach(Jumpable jumpable in jumpables) {
            jumpable.SetController(controller);

            if(jumpable.priority > highestPriority) {
                highestPriority = jumpable.priority;
                this.jumpable = jumpable;
            }
        }
    }

    void SpawnBodyPart(BodyPartComponent part) {
        GameObject pivot = new GameObject("pivot");
        pivot.transform.SetParent(transform, false);
        
        GameObject newPart = Instantiate(part.gameObject, -part.pivot, Quaternion.identity);
        newPart.transform.SetParent(pivot.transform, false);
    }
}