using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureMotor))]
public class PlayerController : MonoBehaviour {

	CreatureMotor motor;

	void Start() {
		motor = GetComponent<CreatureMotor>();
	}

	float lastDirection = 0;

	void Update() {
		//Direction
		if(Input.GetKeyDown(KeyCode.A)) {
			Move(-1f);
		}

		if(Input.GetKeyDown(KeyCode.D)) {
			Move(1f);
		}

		if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
			Move(0f);
		}
	}
	
	private void Jump() {
	}

	private void Move(float dir) {
		motor.Move(dir);
	}

	private void Attack() {

	}
}