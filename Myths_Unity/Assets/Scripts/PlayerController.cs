using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureMotor))]
public class PlayerController : MonoBehaviour {

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	float velocityXSmoothing;
	CreatureMotor motor;

	Vector3 velocity;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	public float wallStickTime = 0.15f;
	float timeToWallUnstick;

	public float moveSpeed = 6;
	public float maxJumpHeight = 2;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.5f ;

	public float wallSlideSpeedMax = 3;

	public float accelerationTimeAirborne = 0.2f;
	public float accelerationTimeGrounded = 0.1f;
	

	void Start() {
		motor = GetComponent<CreatureMotor>();

		gravity = -(2 * maxJumpHeight)/Mathf.Pow(timeToJumpApex,2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
	}

	float lastDirection = 0;

	void Update() {
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		int wallDirX  = (motor.collisions.left)? -1 : 1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (motor.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		
		bool wallSliding = false;
		if((motor.collisions.left || motor.collisions.right) && !motor.collisions.below) {
			wallSliding = true;

			if(velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if(timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

				if(input.x != wallDirX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			} else {
				timeToWallUnstick = wallStickTime;
			}
		}

		if(Input.GetKeyDown(KeyCode.Space)) {
			if(wallSliding){
				if(wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				} else if(input.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if(motor.collisions.below) {
				velocity.y = maxJumpVelocity;
			}
		}
		if(Input.GetKeyUp(KeyCode.Space)) {
			if(velocity.y > minJumpVelocity) {
				velocity.y = minJumpVelocity;
			}
		}

		velocity.y += gravity * Time.deltaTime;

		motor.Move(velocity * Time.deltaTime, input);

		if(motor.collisions.above || motor.collisions.below) {
			velocity.y = 0;
		}

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