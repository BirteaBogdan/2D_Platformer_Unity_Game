using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy //mosteneste clasa enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private LayerMask ground;

    [SerializeField] private float jumpLenght = 10f;
    [SerializeField] private float jumpHeight = 15f;

    private bool facingLeft = true;//initial , animatia este indreptata spre stanga

    private Collider2D coll;
    private Rigidbody2D rb;
    

    protected override void Start()
    {
        base.Start(); // base reprezinta tot ce este mostenit din clasa Enemy
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        //din jump to fall
        if(anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //din fall to idle
        if(coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling",false);
        }
    }
    private void Move()
    {
        if (facingLeft)
        {
            //verificam daca trecem de leftCap
            if (transform.position.x > leftCap)
            {
                //intoarce broasca spre stanga
                if(transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLenght, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }

        else
        {
            //verificam daca trecem de rightCap
            if (transform.position.x < rightCap)
            {
                //face right
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(jumpLenght, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
     
    }

}
