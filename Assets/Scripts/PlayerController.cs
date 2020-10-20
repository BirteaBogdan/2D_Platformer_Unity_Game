using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Start
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D coll;

    private enum State {idle , running , jumping , falling , hurt }
    private State state = State.idle;
    
    //Campuri modificabile din unity
    [SerializeField] private LayerMask Podea;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float hurtForce = 7f;
    [SerializeField] private int cherry = 0;
    [SerializeField] private Text cherryText;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();

    }

    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }
        
        AnimationState();
        anim.SetInteger("state", (int)state);//seteaza animatia in functie de enum State
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.tag == "Cherry")
        {
            Destroy(collision.gameObject);
            cherry += 1;
            cherryText.text = cherry.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    //inamicul este in dreapta player-ului , player takes dmg si se misca catre stanga
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //inamicul este in stanga noastra , player take dmg si ne arunca catre dreapta
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //stanga
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //dreapta
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(Podea))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }
    private void AnimationState()
    {
        if(state == State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(Podea))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)//cat mai aproape de modul 0
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        { //Playerul este in miscare
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
        
    }
}
