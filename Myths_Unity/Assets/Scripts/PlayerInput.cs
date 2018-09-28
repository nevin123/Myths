using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreatureController))]
public class PlayerInput : MonoBehaviour
{  
    CreatureController player;
    
    void Start()
    {
        player = GetComponent<CreatureController>();    
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
