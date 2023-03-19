using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce = 15;
    [SerializeField]
    private Transform checkGround;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float checkGroundRadius = 0.2f;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private GameObject effect;

    private int facingDirection;
    private Rigidbody2D rb;
    private Animator anim;

    public float rotateSpeed = 180f; // Tốc độ xoay (độ/giây)
    private float totalRotation = 0f; // Tổng số độ đã xoay
    private bool isRotate;
    private bool isJump = true;

    private Vector3 localScale;
    private bool moving = true;


    public Transform playerPosition { get; private set; }

    private void Start()
    {
        facingDirection = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localScale = transform.localScale;
    }

    private void Update()
    {
        playerPosition = transform;
        InputManager();
        this.Rotate();
    }

    private void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround())
        {
            //Shoot();
            /*if(!isJump) return;
            soundManager.PlayJumpSound();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("idle", false);
            anim.SetBool("jump", true);*/
            transform.localScale = new Vector3(localScale.x * 3, localScale.y *3, localScale.z *3);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.localScale = localScale;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!isJump) return;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            isJump = !isJump;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isRotate = true;
        }

        if (CheckGround())
        {
            anim.SetBool("jump", false);
        }


        if (moving)
        {
           switch (Input.GetAxisRaw("Horizontal"))
                   {
                       case 1:
                           if(facingDirection != 1)
                           {
                               facingDirection *= -1;
                               transform.Rotate(new Vector3(0f, 180f, 0f));
                           }
                           Move();
                           break;
                       case -1:
                           if (facingDirection != -1)
                           {
                               facingDirection *= -1;
                               transform.Rotate(new Vector3(0f, 180f, 0f));
                           }
                           Move();
                           break;
                       case 0:
                           anim.SetBool("move", false);
                           anim.SetBool("idle", true);
                           break;
                   } 
        }
        else
        {
            anim.SetBool("move", false);

        }
        
    }

    private void Flip()
    {
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }

    private void Move()
    {
        rb.velocity = new Vector2( moveSpeed * facingDirection, rb.velocity.y);
        anim.SetBool("move", true);
        anim.SetBool("idle", false);
        
    }

    private bool CheckGround()
    {
        return Physics2D.OverlapCircle((Vector2)checkGround.position, checkGroundRadius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            gameManager.CollectCoint();
            soundManager.PlayCoinSound();
            Instantiate(effect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
                {
                    transform.Rotate(new Vector3(0, 0, -90));
                    moving = false;
                }
    }

    private void Shoot()
    {
        if(gameManager.currentBulletQuantity <= 0) return;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.facingDirection = facingDirection;
        gameManager.minusBulletQuantity();
        /*Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>().direction = Vector2.up;
        Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>().direction = Vector2.down;
        Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>().direction = Vector2.left;
        Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>().direction = Vector2.right;*/
    }

    private void Rotate()
    {
        if (!isRotate) return;
        // Xoay player một góc nhất định
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        // Cập nhật tổng số độ đã xoay
        totalRotation += rotateSpeed * Time.deltaTime;

        // Nếu đã xoay đủ 2 vòng thì dừng xoay
        if (totalRotation >= 720f)
        {
            rotateSpeed = 0f;
            isRotate = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadius);
    }
}
