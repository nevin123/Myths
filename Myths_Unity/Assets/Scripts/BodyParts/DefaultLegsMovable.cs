using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultLegsMovable : Movable
{
    Controller2D controller;

    //Variables
    float velocityXSmoothing;
    
    //Parameters
    public float moveSpeed;

    public float accelerationTimeAirborne = 0.2f;
	public float accelerationTimeGrounded = 0.1f;

    public void SetControllers(Controller2D controller) {
        this.controller = controller;
    }

    public override void Move(Vector2 input, ref Vector2 velocity) {
        float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
    }
}