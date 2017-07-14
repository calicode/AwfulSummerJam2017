using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float jumpForce;
    public bool isGrounded; //public for testing purposes, change to private once done
    public LayerMask whatIsGround;
    public int startingBottles = 24;

    private int bottles;
    private Rigidbody2D rb;
    private Collider2D myCollider;
    private Animator anim;
    private bool gameStarted;

    [SerializeField]
    private GameObject colRunning;
    [SerializeField]
    private GameObject colSliding;
    [SerializeField]
    private PlatformMover[] startPlatforms;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponentInChildren<Collider2D>();
        anim = GetComponent<Animator>();
        startPlatforms = GameObject.FindObjectsOfType<PlatformMover>();

        Initialize(); //Resets animations to Idle and pauses the game until StartGame input
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);
        anim.SetBool("isGrounded", isGrounded);

        if(!gameStarted)
        {
            if(Input.GetKeyDown(KeyCode.Return)) //Change keycode to something more appropriate once game is near completion
            {
                StartRunning();
            }
        }
        else
        {
            Jump();
            Slide();
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour>();

        if(enemy)
        {
            Debug.Log("Hells bells! I appear to have been hit by this " + enemy.gameObject); //remove this before the game is done
        }
    }

    void StartRunning()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", true);
        gameStarted = true;

        foreach(PlatformMover plats in startPlatforms)
        {
            plats.ResumeGame();
        }
    }

    void Slide()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            if(isGrounded)
            {
                anim.SetBool("isSliding", true);
                colRunning.SetActive(false);
                colSliding.SetActive(true);
                Debug.Log("AY! I'M SLIDIN' OVAH HERE!"); //remove this before the game is done
            }
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("isSliding", false);
            colRunning.SetActive(true);
            colSliding.SetActive(false);
            Debug.Log("AY!!! I'M RUNNIN OVAH HERE!"); //remove this before the game is done
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                anim.SetTrigger("isJumping");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    void IncreaseBottles()
    {
        if(bottles <= 24)
        {
            bottles++;
            //TODO: modify bottle UI text;
        }
        else if(bottles > 24 && bottles < 30)
        {
            bottles++;
            //TODO: modify bottle UI text;
            //TODO: change bottle UI colour to indicate extra bottles;
        }
    }

    void Initialize()
    {
        colRunning.SetActive(true);
        colSliding.SetActive(false);
        gameStarted = false;

        anim.SetBool("isIdle", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isSliding", false);
    }
}
