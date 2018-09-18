using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{   
    public GameObject[] bodyPartsToSpawn;

    //BodyParts
    public IMovable legs;

    void Awake() {
        foreach(GameObject bodyPart in bodyPartsToSpawn) {
            if(bodyPart.GetComponent<IBodyPart>() != null) {
                Instantiate(bodyPart, transform);
            }
        }

        legs = transform.GetComponentInChildren<IMovable>();
    }
}