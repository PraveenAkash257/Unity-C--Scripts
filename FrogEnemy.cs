using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class FrogEnemy : MonoBehaviour
{
    [SerializeField] private float leftcap;
    [SerializeField] private float rightcap;
    [SerializeField] private float JumpLength;
    [SerializeField] private float JumpHeight;
    private bool FacingLeft = true;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private Collider2D col;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        col=GetComponent<Collider2D>();


    }
    void Update()
    {

        if (Mathf.Abs(rb.velocity.x)<0.5)
        {
            anim.SetBool("State",true);
        }
        else
        {
            anim.SetBool("State", false);
        }
        
    }



    private void Move()
    {
        if (transform.position.x > leftcap && col.IsTouchingLayers(ground) && FacingLeft == true)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = new Vector2(-JumpLength, JumpHeight);
        }
        if (transform.position.x <= leftcap)
        {
            FacingLeft = false;
        }
        if (transform.position.x >= rightcap)
        {
            FacingLeft = true;
        }
        if (FacingLeft == false && col.IsTouchingLayers(ground))
        {
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = new Vector2(JumpLength, JumpHeight);
        }

    }
    public void JumpedOn()
    {
        anim.SetTrigger("Die");

    }
    public void death()
    {
        Destroy(gameObject);
    }


}






