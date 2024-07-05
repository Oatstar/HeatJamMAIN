using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    Animator animator;
    SpriteRenderer spriteRen;
    bool previousFlipstate = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRen = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

    }

    public void ResetCharacter()
    {
        this.transform.position = new Vector3(-1.6f, 3.3f, 0);
    }

    void Update()
    {
        if (Time.timeScale <= 0)
            return;

        // Get input from arrow keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (animator != null)
        {
            if (movement.x != 0 || movement.y != 0)
            {
                animator.SetBool("running", true);
            }
            else
            {
                animator.SetBool("running", false);
            }
        }

        if (movement.x < 0)
        {
            spriteRen.flipX = false;
            previousFlipstate = false;

        }
        else if (movement.x > 0)
        {
            spriteRen.flipX = true;
            previousFlipstate = true;
        }
        else
            spriteRen.flipX = previousFlipstate;
    }

    void FixedUpdate()
    {
        // Move the character
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
