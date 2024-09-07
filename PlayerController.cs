using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D RB;
    public Animator anim;
    private Collider2D col;
    [SerializeField]private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float JumpForce = 7f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private float HurtForce = 5f;

     public Text CherryText;

    private enum State {idle,Running,jumping,falling,hurt }
    private State state = State.idle;
    void Start()
    {
        RB= GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {   if(state!=State.hurt)
        {
            Movement();
        }

        AnimationState();
        anim.SetInteger("state", (int)state);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            CherryText.text = cherries.ToString();
            
        }
    }

    private void OnCollisionEnter2D(Collision2D Other)
    {   
        FrogEnemy frog=Other.gameObject.GetComponent<FrogEnemy>();

        if(Other.gameObject.tag=="Enemy")
        {
            if (state == State.falling)
            {
                frog.JumpedOn();
                Jump();
            }
            
            else
            {
                state = State.hurt;
                if (Other.gameObject.transform.position.x>transform.position.x)
                {
                    RB.velocity = new Vector2(-HurtForce, RB.velocity.y);
                    
                }
                else
                {
                    RB.velocity=new Vector2(HurtForce,RB.velocity.y);
                    
                }
            }
        }

    }

    private void Movement()
    {
        float hDirection = UnityEngine.Input.GetAxis("Horizontal");

        //moving left

        if (hDirection < 0)
        {
            RB.velocity = new Vector2(-speed, RB.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }

        //moving right

        else if (hDirection > 0)
        {
            RB.velocity = new Vector2(speed, RB.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }

        //jump

        if (UnityEngine.Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        RB.velocity = new Vector2(RB.velocity.x, JumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (RB.velocity.y < 0.1)
            {
                state = State.falling;
            }
        }
        
        else if (state == State.falling)
        {
            if(col.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state==State.hurt)
        {
            if(Mathf.Abs(RB.velocity.x)<0.5f)
            {
                state = State.idle;
            }
        }

        else if(Mathf.Abs(RB.velocity.x)>2f)
        {
            state = State.Running;
        }
        else
        {
            state = State.idle;
        }
    }
}
