using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController PlayerController;
    private Animator playerAnimator;
    private float horizontalMove = 0f;   
    private bool jump = false;


    [Range(0.1f, 1f)] [SerializeField] private float runSpeed = 1f;
    [Range(1, 4f)] [SerializeField] private float jumpForce = 2.5f;  

    private void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {       
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        playerAnimator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            playerAnimator.SetBool("Jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) //костыль, чтобы избежать повторной анимации прыжка в воздухе при повторном нажатии Space
    {   
        if (collision.gameObject.layer == 6)    //слой Ground
        {
            playerAnimator.SetBool("Jump", false);
        }
    }

    private void FixedUpdate()
    {
        PlayerController.Move(horizontalMove , jump, jumpForce);
        jump = false;
    }
}
