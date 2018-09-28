using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{   
    public GameObject[] bodyPartsToSpawn;

    //BodyParts
    public Movable movable;

    void Awake() {
        foreach(GameObject bodyPart in bodyPartsToSpawn) {
            if(bodyPart.GetComponent<BodyPartComponent>() != null) {
                Instantiate(bodyPart, transform);
            }
        }

        movable = transform.GetComponentInChildren<Movable>();
    }
}