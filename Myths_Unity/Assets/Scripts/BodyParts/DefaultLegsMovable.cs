using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegsMovable : Movable
{
    //Variables
    float velocityXSmoothing;
    
    //Parameters
    public float moveSpeed;

    public float accelerationTimeAirborne = 0.2f;
	public float accelerationTimeGrounded = 0.1f;

    public override void Move(ref Vector2 velocity, Vector2 input) {
        float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
    }
}