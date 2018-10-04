using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegsJumpable : Jumpable {

    //Variables
    bool holdingJumpButton = false;

    float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;

    bool wallSliding;
	int wallDirX;

    //Parameters
    public float maxJumpHeight = 2;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.5f;

    void Start() {
        gravity = -(2 * maxJumpHeight)/Mathf.Pow(timeToJumpApex,2);
		maxJumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2*Mathf.Abs(gravity) * minJumpHeight);
    }

    public override void Move(ref Vector2 velocity) {

        if(velocity.y >= 0 && holdingJumpButton) {
			velocity.y +=  gravity * Time.deltaTime;
		} else {
			velocity.y += 2 * gravity * Time.deltaTime;
		}
    }

    public override void OnJumpButtonDown(ref Vector2 velocity, Vector2 input) {
        holdingJumpButton = true;

        if(controller.collisions.below) {
			if(controller.collisions.slidingDownMaxSlope) {
				if(input.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) {
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
    }

    public override void OnJumpButtonUp(ref Vector2 velocity) {
        holdingJumpButton = false;

		if(velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
    }
}