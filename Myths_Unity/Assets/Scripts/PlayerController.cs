using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureMotor))]
public class PlayerController : MonoBehaviour {

	float gravity;
	float jumpVelocity;
	float velocityXSmoothing;
	CreatureMotor motor;

	Vector3 velocity;

	public float moveSpeed = 6;
	public float jumpHeight = 2;
	public float timeToJumpApex = 0.5f ;

	public float accelerationTimeAirborne = 0.2f;
	public float accelerationTimeGrounded = 0.1f;
	

	void Start() {
		motor = GetComponent<CreatureMotor>();

		gravity = -(2 * jumpHeight)/Mathf.Pow(timeToJumpApex,2);
		jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
	}

	float lastDirection = 0;

	void Update() {
		if(motor.collisions.above || motor.collisions.below) {
			velocity.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(Input.GetKeyDown(KeyCode.Space) && motor.collisions.below) {
			velocity.y = jumpVelocity;
		}


		float targetVelocityX = input.x * moveSpeed;
		
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (motor.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
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