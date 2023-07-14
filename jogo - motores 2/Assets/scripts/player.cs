using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public GameObject bala;
    public Transform firePoint;
    
    private bool isJumping;
    private bool doubleJump;
    private bool isAtack;

    private Rigidbody2D rig;
    private Animator anim;

    private float movement;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Atack();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    { 
        movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1); 
            }
           
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isAtack)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if (doubleJump)
                {
                    anim.SetInteger("transition", 2);
                    rig.AddForce(new Vector2(0, jumpForce * 1), ForceMode2D.Impulse);
                    doubleJump = false; 
                }
            }
           
        }
        
    }

    void Atack()
    {
        StartCoroutine("Atk");
    }

    IEnumerator Atk()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isAtack = true;
            anim.SetInteger("transition", 3);
            GameObject Bala = Instantiate(bala, firePoint.position, firePoint.rotation);

            if (transform.rotation.y == 0)
            {
                Bala.GetComponent<bala>().isRight = true;
            }

            if (transform.rotation.y == 180)
            {
                Bala.GetComponent<bala>().isRight = false;
            }
            
            yield return new WaitForSeconds(0.5f);
            isAtack = false;
            anim.SetInteger("transition", 0);
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }
}
