using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection;
    public float speed = 5f;
    private float gravity = 20f;
    public float jumpForce = 10f;
    private float verticalVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        moveDirection = new Vector3(
                                        Input.GetAxis("Horizontal"), 
                                        0, 
                                        Input.GetAxis("Vertical")
                                   );
        moveDirection = transform.TransformDirection(moveDirection);

        moveDirection *= speed * Time.deltaTime;
        JumpPlayer();
        characterController.Move(moveDirection);
    }

    void JumpPlayer()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
        }

        verticalVelocity -= gravity * Time.deltaTime;
        moveDirection.y = verticalVelocity * Time.deltaTime;
    }
}
