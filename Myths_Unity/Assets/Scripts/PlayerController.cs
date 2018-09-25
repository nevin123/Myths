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

	Vector2 directionalInput;
	
	bool wallSliding;
	int wallDirX;

	void Start() {
		motor = GetComponent<CreatureMotor>();

		gravity = -(2 * maxJumpHeight)/Mathf.Pow(timeToJumpApex,2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update() {
		CalculateVelocity();
		HandleWallSliding();
		
		motor.Move(velocity * Time.deltaTime, directionalInput);

		if(motor.collisions.above || motor.collisions.below ) {
			if(motor.collisions.slidingDownMaxSlope) {
				velocity.y += motor.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput(Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		if(wallSliding && !motor.collisions.slidingDownMaxSlope){
			if(wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			} else if(directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			} else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
		if(motor.collisions.below) {
			if(motor.collisions.slidingDownMaxSlope) {
				if(directionalInput.x != -Mathf.Sign(motor.collisions.slopeNormal.x)) {
					velocity.y = maxJumpVelocity * motor.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * motor.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if(velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (motor.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	void HandleWallSliding() {
		wallDirX  = (motor.collisions.left)? -1 : 1;
		wallSliding = false;
		
		if((motor.collisions.left || motor.collisions.right) && !motor.collisions.below && !motor.collisions.slidingDownMaxSlope) {
			wallSliding = true;
			Debug.Log("wallSliding");

			if(velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if(timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				// velocity.x = 0;

				if(directionalInput.x != wallDirX && directionalInput.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			}
		} else {
			timeToWallUnstick = wallStickTime;
		}
	}
}