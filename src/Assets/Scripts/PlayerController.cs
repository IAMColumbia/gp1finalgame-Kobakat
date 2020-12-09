using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Player player;
    float input;
    float absoluteInput;

    #region InputHandler Singleton
    private static InputHandler instance;

    public static InputHandler Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new InputHandler();
                instance.Enable();
            }

            return instance;
        }
    }

    #endregion

    #region Delegate Subscription Management
    void OnEnable()
    {
        Instance.Map.Jump.started += OnJump;
        Instance.Map.Jump.performed += OnJumpPeak;
        Instance.Map.Jump.canceled += OnJumpRelease;
    }

    void OnDisable()
    {
        Instance.Map.Jump.started -= OnJump;
        Instance.Map.Jump.performed -= OnJumpPeak;
        Instance.Map.Jump.canceled -= OnJumpRelease;
    }

    #endregion

    #region Unity Message Functions
    void Start()
    {
        player = this.GetComponent<Player>();
        instance.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        input = Instance.Map.Movement.ReadValue<float>();
        absoluteInput = Mathf.Abs(input);

        if(player.gameState is PlayState)
        {
            if (absoluteInput > 0)
            {
                player.xMoveDir = input;

                if (!(player.moveState is MovementState) && !(player.moveState is TurningState))
                    player.SetState(ref player.moveState, new MovementState(player));
            }

            else
            {
                if (!(player.moveState is StoppingState))
                    player.SetState(ref player.moveState, new StoppingState(player));
            }
        }
        
    }

    #endregion

    #region Input Event Logic
    void OnJump(InputAction.CallbackContext context)
    {
        if(player.groundState is GroundedState)
            player.SetState(ref player.groundState, new JumpingState(player)); 
    }

    void OnJumpPeak(InputAction.CallbackContext context)
    {
        if ((player.groundState is JumpingState))
            player.SetState(ref player.groundState, new RisingState(player));

    }

    void OnJumpRelease(InputAction.CallbackContext context)
    {
        if ((player.groundState is JumpingState))
            player.SetState(ref player.groundState, new RisingState(player));
    }

    #endregion
}
