using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float jumpForce;
    public bool isGrounded; //public for testing purposes, change to private once done
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Collider2D myCollider;
    private Animator anim;

    [SerializeField]
    private GameObject colRunning;
    [SerializeField]
    private GameObject colSliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponentInChildren<Collider2D>();
        anim = GetComponent<Animator>();

        Initialize();
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);
        anim.SetBool("isGrounded", isGrounded);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                anim.SetTrigger("isJumping");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        if(Input.GetKey(KeyCode.Q))
        {
            if(isGrounded)
            {
                anim.SetBool("isSliding", true);
                colRunning.SetActive(false);
                colSliding.SetActive(true);
                Debug.Log("AY! I'M SLIDIN' OVAH HERE!");
            }
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("isSliding", false);
            colRunning.SetActive(true);
            colSliding.SetActive(false);
            Debug.Log("AY!!! I'M RUNNIN OVAH HERE!");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour>();

        if(enemy)
        {
            Debug.Log("Hells bells! I appear to have been hit!");
        }
    }


    void Initialize()
    {
        colRunning.SetActive(true);
        colSliding.SetActive(false);

        anim.SetBool("isIdle", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isSliding", false);
    }
}
