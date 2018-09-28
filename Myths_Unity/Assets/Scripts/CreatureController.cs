using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(Controller2D))]
public class CreatureController : MonoBehaviour {

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	float velocityXSmoothing;
	Controller2D controller;

	Vector3 velocity;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;
	public float wallStickTime = 0.15f;
	float timeToWallUnstick;

	public float moveSpeed = 6;
	public float maxJumpHeight = 2;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.5f;

	public float wallSlideSpeedMax = 3;

	public float accelerationTimeAirborne = 0.2f;
	public float accelerationTimeGrounded = 0.1f;

	Vector2 directionalInput;
	
	bool holdingJumpButton = false;
	bool wallSliding;
	int wallDirX;

	void Start() {
		controller = GetComponent<Controller2D>();

		gravity = -(2 * maxJumpHeight)/Mathf.Pow(timeToJumpApex,2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update() {
		CalculateVelocity();
		HandleWallSliding();
		
		controller.Move(velocity * Time.deltaTime, directionalInput);

		if(controller.collisions.above || controller.collisions.below ) {
			if(controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	public void SetDirectionalInput(Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		holdingJumpButton = true;

		if(wallSliding && !controller.collisions.slidingDownMaxSlope){
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
		if(controller.collisions.below) {
			if(controller.collisions.slidingDownMaxSlope) {
				if(directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) {
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		holdingJumpButton = false;

		if(velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	void CalculateVelocity() {
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		
		if(velocity.y > 0 && holdingJumpButton) {
			velocity.y +=  gravity * Time.deltaTime;
		} else {
			velocity.y += 2 * gravity * Time.deltaTime;
		}
	}

	void HandleWallSliding() {
		wallDirX  = (controller.collisions.left)? -1 : 1;
		wallSliding = false;
		
		if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && !controller.collisions.slidingDownMaxSlope) {
			wallSliding = true;

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