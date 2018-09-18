using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature))]
public class CreatureMotor : MonoBehaviour {

    public float direction = 0;
    float newDirection = 0;
    float velocity;

    bool isJumping = false;
    bool isAttacking = false;

    Creature creature;

    void Start() {
        creature = GetComponent<Creature>();
    }

    void Update() {
        direction = Mathf.SmoothDamp(direction, newDirection, ref velocity, Time.deltaTime * 10, 100);
        creature.legs.Move(direction);
    }

	public void Move(float direction) {
        newDirection = direction;

        if(direction < 0) {
            transform.localScale = new Vector3(-1,1,1);
        } else if(direction > 0){
            transform.localScale = new Vector3(1,1,1);
        }
    }

    public void Jump(bool isJumping) {
        this.isJumping = isJumping;
    }

    public void Attack(bool isAttacking) {
        this.isAttacking = isAttacking;   
    }
}
