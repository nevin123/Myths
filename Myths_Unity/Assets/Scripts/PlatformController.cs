﻿using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class PlatformController : RaycastController
{   
    public LayerMask passengerMask;

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints; 

    public float speed = 3;
    public bool cyclic = false;
    public float waitTime = 0;
    [Range(0,2)]
    public float easeAmount = 1;

    int fromWayPointIndex;
    float percentageBetweenWaypoints;
    float nextMoveTime;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    public override void Start() {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for(int i = 0; i < localWaypoints.Length; i++) {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    void Update() {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        
        CalculatePassengerMovement(velocity);


        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    float Ease(float x) {
        float a = easeAmount + 1;

        return Mathf.Pow(x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1-x,a)); 
    }

    Vector3 CalculatePlatformMovement() {
        if(Time.time < nextMoveTime) {
            return Vector3.zero;
        }

        fromWayPointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWayPointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWayPointIndex], globalWaypoints[toWaypointIndex]);

        percentageBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentageBetweenWaypoints = Mathf.Clamp01(percentageBetweenWaypoints);

        float easedPercentageBetweenPoints = Ease(percentageBetweenWaypoints); 
        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWayPointIndex], globalWaypoints[toWaypointIndex], easedPercentageBetweenPoints);

        if (percentageBetweenWaypoints >= 1) {
            percentageBetweenWaypoints = 0;
            fromWayPointIndex++;

            if(!cyclic) {
                if(fromWayPointIndex >= globalWaypoints.Length -1) {
                    fromWayPointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }

            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform) {
        foreach(PassengerMovement passenger in passengerMovement) {
            if(passenger.moveBeforePlatformIsMoved == beforeMovePlatform) {
                if(!passengerDictionary.ContainsKey(passenger.transform)) {
                   passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>()); 
                }
                passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform); 
            }
        }
    }

    void CalculatePassengerMovement(Vector3 velocity) {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //vertically moving platform
        if(velocity.y != 0) {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for(int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

                if(hit && hit.distance != 0) {
                    if(!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);

                        float pushX = (directionY == 1)?velocity.x:0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        if(velocity.x != 0) {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for(int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

                if(hit && hit.distance != 0) {
                    if(!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x  - (hit.distance - skinWidth) * directionX;
                        float pushY = -skinWidth;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }
            }
        }

        //Passenger on top of a horizontally or downward moving platform
        if(directionY == -1 || velocity.y == 0 && velocity.x != 0) {
            float rayLength = skinWidth * 2;

            for(int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

                if(hit && hit.distance != 0) {
                    if(!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);

                        float pushX = velocity.x;
                        float pushY = velocity.y ;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }
    }

    struct PassengerMovement {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatformIsMoved;

        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatformIsMoved) {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatformIsMoved = _moveBeforePlatformIsMoved;
        }
    }

    void OnDrawGizmos() {
        if(localWaypoints != null) {
            Gizmos.color = Color.red;
            float size = 0.3f;
            
            for(int i = 0; i < localWaypoints.Length; i++) {
                Vector3 globalWaypointPosition = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position;

                Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);
            }
        }
    }
}
