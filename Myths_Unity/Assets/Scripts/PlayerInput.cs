﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{  
    PlayerController player;
    
    void Start()
    {
        player = GetComponent<PlayerController>();    
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        player.SetDirectionalInput(directionalInput);

        if(Input.GetKeyDown(KeyCode.Space)) {
            player.OnJumpInputDown();
            // Debug.Log("pressedSpace");
        }

        if(Input.GetKeyUp(KeyCode.Space)) {
            player.OnJumpInputUp();
        }
    }
}
