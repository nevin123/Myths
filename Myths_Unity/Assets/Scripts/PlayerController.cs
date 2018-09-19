using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureMotor))]
public class PlayerController : MonoBehaviour {

	CreatureMotor motor;

	float gravity = -20;
	Vector3 velocity;

	public float moveSpeed = 6;

	void Start() {
		motor = GetComponent<CreatureMotor>();
	}

	float lastDirection = 0;

	void Update() {
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		velocity.x = input.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		motor.Move(velocity * Time.deltaTime);

		// //Direction
		// if(Input.GetKeyDown(KeyCode.A)) {
		// 	Move(-1f);
		// }

		// if(Input.GetKeyDown(KeyCode.D)) {
		// 	Move(1f);
		// }

		// if(!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) {
		// 	Move(0f);
		// }
	}
	
	// private void Jump() {
	// }

	// private void Move(float dir) {
	// 	motor.Move(dir);
	// }

	// private void Attack() {

	// }
}