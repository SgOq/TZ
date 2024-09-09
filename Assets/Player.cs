using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 moveVector;
    public float speed = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        WallCheckRadius = WallCheck.GetComponent<CircleCollider2D>().radius;
        gravityDef = rb.gravityScale;

    }

    void Update()
    {
        walk();
        CheckingWall();
        MoveOnWall();
    }

    void walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.AddForce(moveVector*speed);
    }

    public bool onWall;
    public LayerMask Wall;
    public Transform WallCheck;
    private float WallCheckRadius;

    void CheckingWall()
    {
        onWall = Physics2D.OverlapCircle(WallCheck.position, WallCheckRadius, Wall);
    }

    public float upDownSpeed = 4f;
    public float slideSpeed = 0;
    private float gravityDef;

    void MoveOnWall() 
    {
        if (onWall) 
        {
            moveVector.y = Input.GetAxisRaw("Vertical");

             //////////////////////////
            if(moveVector.y == 0)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2 (0, slideSpeed);
            }
            ///////////////////////////
            
            if (moveVector.y >0)
            {
                rb.velocity =   new Vector2(rb.velocity.x, moveVector.y * upDownSpeed);
            }
            else if (moveVector.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveVector.y * upDownSpeed);
            }
        }
        //else if(!onWall)
        //{
            //rb.gravityScale = gravityDef;
        //}
    }
}