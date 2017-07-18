using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public float jumpForce;
    public bool isGrounded; //public for testing purposes, change to private once done
    public LayerMask whatIsGround;
    public int initBottles = 24; //set to private once testing is over
    public float initBoozeTimer = 5f; //set to private once testing is over

    private Vector3 initPos;
    private float boozeTimer;
    private int bottles;
    private Rigidbody2D rb;
    private Collider2D myCollider;
    private Animator anim;
    private bool gameStarted;
    private bool boozedUp;
    private bool dead = false;

    [SerializeField]
    private GameObject colRunning;
    [SerializeField]
    private GameObject colSliding;
    [SerializeField]
    private GameObject colBoozePower;
    [SerializeField]
    private PlatformMover[] startPlatforms;
    [SerializeField]
    private Text bottleCountText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponentInChildren<Collider2D>();
        anim = GetComponent<Animator>();
        startPlatforms = GameObject.FindObjectsOfType<PlatformMover>();
        boozeTimer = initBoozeTimer;
        bottles = initBottles;
        initPos = transform.position;

        UpdateBottleCountDisplay();
        Initialize(); //Resets animations to Idle and pauses the game until StartGame input
    }

    void Update()
    {
        isGrounded = Physics2D.IsTouchingLayers(myCollider, whatIsGround);
        anim.SetBool("isGrounded", isGrounded);

        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Return)) //Change keycode to something more appropriate once game is near completion
            {
                StartRunning();
            }
        }
        else if(dead)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                
                foreach (PlatformMover plats in startPlatforms)
                {
                    plats.ResetPos();
                    plats.gameObject.SetActive(true);
                }
                dead = false;
                Initialize();
                //TODO: Reset ALL enemy Positions as well;
            }
        }
        else
        {
            Jump();
            Slide();
            ActivateBoozePower();

            if(boozedUp)
            {
                boozeTimer -= Time.deltaTime;

                if(boozeTimer <= 0)
                {
                    DeactivateBoozePower();
                }
            }

        }



    }

    void StartRunning()
    {
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", true);
        gameStarted = true;

        foreach (PlatformMover plats in startPlatforms)
        {
            plats.ResumeGame();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour>();

        if (enemy)
        {
            anim.SetBool("isDead", true);
            enemy.StopMoving();
            foreach (PlatformMover plats in startPlatforms)
            {
                plats.PauseGame();
            }
            dead = true;

            Debug.Log("Hells bells! I appear to have been hit by this " + enemy.gameObject); //remove this before the game is done
        }
    }

    void ActivateBoozePower()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            boozedUp = true;
            colBoozePower.SetActive(true);
        }
        //TODO: Need to make a TriggerCollider Script for BoozePower
    }

    void DeactivateBoozePower()
    {
        boozedUp = false;
        colBoozePower.SetActive(false);
        boozeTimer = initBoozeTimer;
    }



    void Slide()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (isGrounded)
            {
                anim.SetBool("isSliding", true);
                colRunning.SetActive(false);
                colSliding.SetActive(true);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("isSliding", false);
            colRunning.SetActive(true);
            colSliding.SetActive(false);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                anim.SetTrigger("isJumping");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                IncreaseBottles(); // just testing
            }
        }
    }

    void IncreaseBottles()
    {
        if (bottles < 30)
        {
            bottles++;
            UpdateBottleCountDisplay();
        }
    }

    void UpdateBottleCountDisplay()
    {
        if (bottles <= 24)
        {

            bottleCountText.color = Color.white;
        }
        else if (bottles > 24)
        {
            bottleCountText.color = Color.green;
        }

        /* else if (bottles < minPitStopBottleCount)
         {
             // add a visual indicator, maybe text size pulses
             bottleCountText.color = Color.red;

         }
 */

        bottleCountText.text = bottles.ToString();

    }

    void Initialize()
    {
        colRunning.SetActive(true);
        colBoozePower.SetActive(false);
        colSliding.SetActive(false);
        gameStarted = false;
        boozedUp = false;
        dead = false;
        transform.position = initPos;

        anim.SetBool("isDead", false);
        anim.SetBool("isIdle", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isSliding", false);
    }
}
