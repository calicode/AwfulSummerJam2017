using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public float jumpForce;
    public bool isGrounded; //public for testing purposes, change to private once done
    public LayerMask whatIsGround; //Checks to see if the Layer is marked "ground"
    public int initBottles = 24; //set to private once testing is over  //Starting Bottle count
    public float initBoozeTimer = 5f; //set to private once testing is over  //Starting BoozeTimer
    public int minBottleReq = 5; //The minimal bottle requirement for the pitstop

    private Vector3 initPos; //Starting Player Position
    private float boozeTimer; //Current Booze Timer
    private int bottles; //Current Bottle count
    private Rigidbody2D rb; //The player's Rigidbody
    [SerializeField]private Collider2D runCollider; //The player's running collider
    [SerializeField]private Collider2D slideCollider; //the player's slide collider
    private Animator anim; //The Player's animator
    private bool gameStarted; //Is the game currently going?
    private bool boozedUp; //Is the player currently in Booze Power mode?
    private bool dead = false; //Well pretty self explanatory
    private bool isSliding = false;

    //Bunch of variables I want to see in editor but not change
    [SerializeField]
    private GameObject colRunning; //The running collider
    [SerializeField]
    private GameObject colSliding; //The sliding collider
    [SerializeField]
    private GameObject colBoozePower; //The booze power collider
    [SerializeField]
    private PlatformMover[] startPlatforms; //All the platforms currently in the scene
    [SerializeField]
    private EnemyBehaviour[] enemies; //All the enemies currently in the scene
    [SerializeField]
    private Text bottleCountText; //The bottle count text............

    void Start()
    {
        //Yeh git those components
        rb = GetComponent<Rigidbody2D>();
        runCollider = colRunning.GetComponent<Collider2D>();
        slideCollider = colSliding.GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        startPlatforms = GameObject.FindObjectsOfType<PlatformMover>();
        enemies = GameObject.FindObjectsOfType<EnemyBehaviour>();

        //Initializes the timer, count and position (Should this be in the Initialize function? TBD)
        boozeTimer = initBoozeTimer;
        bottles = initBottles;
        initPos = transform.position;

        UpdateBottleCountDisplay(); //Updates...the Bottle...Count...Display
        Initialize(); //Resets animations to Idle and pauses the game until StartGame input
    }

    void Update()
    {
        //What is ground????? This checks to see if the player is touching it!    
        if(!isSliding)
        {
            isGrounded = Physics2D.IsTouchingLayers(runCollider, whatIsGround);         }
        else
        {
            isGrounded = Physics2D.IsTouchingLayers(slideCollider, whatIsGround);
        }

        anim.SetBool("isGrounded", isGrounded); //Makes sure the animations go back to normal if the player touches the ground

        if (!gameStarted) //Checks to see if the game is set to "pause" mode
        {
            if (Input.GetKeyDown(KeyCode.Return)) //Change keycode to something more appropriate once game is near completion
            {
                StartRunning(); //Starts the game!
            }
        }
        else if(dead) //Checks to see if you died a horrible painful death
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                ResetEverything(); //Three guesses as to what this does
            }
        }
        else //This should be the default setting, when the game is running
        {
            Jump(); //Lets you soar through the air without a care in the world
            Slide(); //Lets you slide real smooth like
            ActivateBoozePower(); //Activates the wonderful magical power of BOOZE
            Throw();
            Punch();

            //The booze power timer
            if(boozedUp) //Are you drunk?
            {
                boozeTimer -= Time.deltaTime;

                if(boozeTimer <= 0)
                {
                    DeactivateBoozePower(); //You are now sober/sad
                }
            }

        }



    }

    //This happens when you touch a thing
    public void Dead()
    {
        //TODO: Needs a condition if you touch an enemy VS when you fall down a pit

        anim.SetBool("isDead", true);
        foreach(PlatformMover plats in startPlatforms)
        {
            plats.PauseGame();
        }
        foreach(EnemyBehaviour bads in enemies)
        {
            bads.StopMoving();
        }
        dead = true;
        isSliding = false;
    }

    //This is the function that puts the game in motion
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

    //The allmighty collision detector, if you hit something, you are dead!
    void OnCollisionEnter2D(Collision2D col)
    {
        EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour>();

        if (enemy) //Makes sure you don't die when you touch the ground
        {
            Dead();
        }
    }

    //Drink up, be merry, and also invincible for a couple of seconds (pits don't care if you're drunk)
    void ActivateBoozePower()
    {
        if(Input.GetKeyDown(KeyCode.E) && !boozedUp)
        {
            bottles -= 5;
            UpdateBottleCountDisplay();
            boozedUp = true;
            colBoozePower.SetActive(true);
        }
        //TODO: Need to make a TriggerCollider Script for BoozePower
    }

    //You sober up real quick, and you lose your invincibility
    void DeactivateBoozePower()
    {
        boozedUp = false;
        colBoozePower.SetActive(false);
        boozeTimer = initBoozeTimer;
    }



    //Puts everything back where it was, all neat and tidy
    void ResetEverything()
    {
        foreach(PlatformMover plats in startPlatforms)
        {
            plats.ResetPos();
            plats.gameObject.SetActive(true);
        }
        foreach(EnemyBehaviour bads in enemies)
        {
            bads.ResetPosition();
        }
        dead = false;
        Initialize();
    }

    //Changes the running collider to the slide collider, also makes you look real cool
    void Slide()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            anim.SetBool("isSliding", true);
            colRunning.SetActive(false);
            colSliding.SetActive(true);
            isSliding = true;    
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            anim.SetBool("isSliding", false);
            colRunning.SetActive(true);
            colSliding.SetActive(false);
            isSliding = false;
        }
    }

    //Gets you really high
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //You can't jump while you're already jumping, dummy!
            if (isGrounded)
            {
                anim.SetTrigger("isJumping");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                IncreaseBottles(); // just testing
            }
        }
    }

    //Increases your booze count
    void IncreaseBottles()
    {
        if (bottles < 30)
        {
            bottles++;
            UpdateBottleCountDisplay();
        }
    }

    //Remove bottles when you're hit
    void DecreaseBottles()
    {
        if(bottles > 0)
        {
            bottles -= 5;
            UpdateBottleCountDisplay();
        }
    }

    //Throws a bottle
    void Throw()
    {
        if(Input.GetKeyDown(KeyCode.W)) //Set to something better
        {
            anim.SetTrigger("isThrowing");
            //TODO: Instantiate bottle
            bottles--;
            UpdateBottleCountDisplay();
        }

    }

    //Punchs a human enemy (?)
    void Punch()
    {
        if(Input.GetKeyDown(KeyCode.R)) //Set to something better
        {
            anim.SetTrigger("isPunching");
            //TODO: Make a collider (or use the boozed up collider?) and use it
        }
    }

    //Updates your booze count, white text if in normal range, green if above 24 and red if below...5?
    void UpdateBottleCountDisplay()
    {
        if(bottles <= minBottleReq)
        {
            bottleCountText.color = Color.red;
            //Add a pulsating thing to the text to INDICATE URGENCY!!!!!
        }
        else if (bottles <= 24)
        {
            bottleCountText.color = Color.white;
        }
        else if (bottles > 24)
        {
            bottleCountText.color = Color.green;
        }

        bottleCountText.text = bottles.ToString();

    }

    //Sets all the colliders, animations, positions and conditions for the player back to normal.
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
