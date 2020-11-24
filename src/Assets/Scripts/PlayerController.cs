using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Player player;
    float input;
    float absoluteInput;

    void OnEnable()
    {
        InputHandler.Instance.Map.Jump.performed += OnJump;
    }

    void OnDisable()
    {
        InputHandler.Instance.Map.Jump.performed -= OnJump;  
    }
    void Start()
    {
        player = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        input = InputHandler.Instance.Map.Movement.ReadValue<float>();
        absoluteInput = Mathf.Abs(input);

        if(absoluteInput > 0)
        {
            player.xMoveDir = input;

            if (!(player.moveState is MovementState))
                player.SetState(ref player.moveState, new MovementState(player));
        }

        else
        {
            if (!(player.moveState is StoppingState))
                player.SetState(ref player.moveState, new StoppingState(player));
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (player.groundState is GroundedState)
            player.SetState(ref player.groundState, new JumpingState(player));

        else
        {
            if (!player.hasDoubleJumped)
            {
                player.SetState(ref player.groundState, new JumpingState(player));
                player.hasDoubleJumped = true;
            }
        }
    }
}
