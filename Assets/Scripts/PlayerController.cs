using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputVector;
    public float playerSpeed;
    public Scanner scanner;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    Animator playerAnimator;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }

    void FixedUpdate()
    {
        Vector2 moveVet = inputVector * playerSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + moveVet);
    }

    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
    
    void LateUpdate()
    {
        if(inputVector.x != 0)
        {
            spriteRenderer.flipX = inputVector.x > 0; //비교연산자
        }

        playerAnimator.SetFloat("playerSpeed", inputVector.magnitude);
    }
}
