using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(Controller2D))]
public class CreatureController : MonoBehaviour {

	Creature creature;
	Controller2D controller;

	Vector2 velocity;
	Vector2 directionalInput;
	
	void Start() {
		creature = GetComponent<Creature>();
		controller = GetComponent<Controller2D>();
	}

	void Update() {
		creature.movable.Move(ref velocity, directionalInput);
		creature.jumpable.Move(ref velocity);

		controller.Move(velocity * Time.deltaTime, directionalInput);

		if(controller.collisions.above || controller.collisions.below ) {
			if(!controller.collisions.slidingDownMaxSlope) {
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput(Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		creature.jumpable.OnJumpButtonDown(ref velocity, directionalInput);
	}

	public void OnJumpInputUp() {
		creature.jumpable.OnJumpButtonUp(ref velocity);
	}
}