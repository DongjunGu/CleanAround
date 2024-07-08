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
        if (!GameManager.instance.isLive)
            return;
        inputVector = value.Get<Vector2>();
    }
    
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;
        if (inputVector.x != 0)
        {
            spriteRenderer.flipX = inputVector.x > 0; //비교연산자   
        }

        playerAnimator.SetFloat("playerSpeed", inputVector.magnitude);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;
        GameManager.instance.health -= Time.deltaTime * 10;

        if(GameManager.instance.health < 0)
        {
            for(int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            playerAnimator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
