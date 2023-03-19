using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float maxDistance = 5f;

    private Vector2 startPos;
    public float facingDirection;
    public Vector2 direction;

    private Animator anim;
    private void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Return();
        transform.Translate(transform.right * facingDirection * speed * Time.deltaTime);

        //transform.Translate((Vector3)direction * speed * Time.deltaTime);
        Invoke("Anim", 2);
    }

    private void Return()
    {
        transform.Translate(transform.right * facingDirection * speed * Time.deltaTime);
        if (Vector2.Distance(startPos, transform.position) > maxDistance)
        {
            facingDirection *= -1;
            startPos = transform.position;
            Destroy(gameObject, 0.5f);

        }
    }

    private void Anim()
    {
        anim.SetBool("destroy", true);
    }

    private void DesTroy()
    {
        Destroy(gameObject);
    }
}
