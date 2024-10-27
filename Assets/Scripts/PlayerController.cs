using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputVector;
    public float playerSpeed;
    public Scanner scanner;
    public Animator playerAnimator;
    public CapsuleCollider2D capCollider;
    public Joystick joystick;
    public RuntimeAnimatorController[] anims;

    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        capCollider = GetComponent<CapsuleCollider2D>();
    }
    void Update()
    {
        inputVector.x = joystick.Horizontal;
        inputVector.y = joystick.Vertical;
    }
    void FixedUpdate()
    {
        Vector2 moveVet = inputVector.normalized * playerSpeed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + moveVet);
    }
    public void ChangeCharacter(int id)
    {
        GameManager.instance.playerId = id;
        playerAnimator.runtimeAnimatorController = anims[GameManager.instance.playerId];
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
        if(collision.gameObject.tag == "Enemy")
            GameManager.instance.health -= Time.deltaTime * 30;

        if (GameManager.instance.health < 0)
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
