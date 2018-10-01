using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{   
    public GameObject[] bodyPartsToSpawn;

    //BodyParts
    public BodyPartComponent[] allBodyParts;
    public Movable[] movable;
    public Body[] body;

    void Awake() {
        foreach(GameObject bodyPart in bodyPartsToSpawn) {
            if(bodyPart.GetComponent<BodyPartComponent>() != null) {
                Instantiate(bodyPart, transform);
            }
        }

        allBodyParts = transform.GetComponentsInChildren<BodyPartComponent>();
        movable = transform.GetComponentsInChildren<Movable>();
        body = transform.GetComponentsInChildren<Body>();
    }
}