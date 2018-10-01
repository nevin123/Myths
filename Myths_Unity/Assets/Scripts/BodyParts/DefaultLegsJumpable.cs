using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegsJumpable : Jumpable {
    
    //Variables
    bool holdingJumpButton = false;

    float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;

    //Parameters
    public float maxJumpHeight = 2;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.5f;

    public override void Move(ref Vector2 velocity) {

        if(velocity.y > 0 && holdingJumpButton) {
			velocity.y +=  gravity * Time.deltaTime;
		} else {
			velocity.y += 2 * gravity * Time.deltaTime;
		}
    }

    public override void OnJumpButtonDown() {
        holdingJumpButton = true;
    }

    public override void OnJumpButtonUp() {
        holdingJumpButton = false;
    }
}