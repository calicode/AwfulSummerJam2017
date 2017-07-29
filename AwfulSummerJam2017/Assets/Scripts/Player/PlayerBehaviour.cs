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
    public int minBottleReq = 10; //The minimal bottle requirement for the pitstop

    private Vector3 initPos; //Starting or checkpoint Player Position
    private float boozeTimer; //Current Booze Timer
    private int bottles; //Current Bottle count
    private Rigidbody2D rb; //The player's Rigidbody
    [SerializeField] private Collider2D runCollider; //The player's running collider
    [SerializeField] private Collider2D slideCollider; //the player's slide collider
    private Animator anim; //The Player's animator
    private bool gameStarted; //Is the game currently going?
    private bool boozedUp; //Is the player currently in Booze Power mode?
    private bool dead = false; //Well pretty self explanatory
    private bool isSliding = false;
    private bool isPunching = false;
    private bool readyThrow = true;
    private bool pitStopTouched = false;
    private bool gameEnd = false;
    private SFXManager sfxManager;
    private int jumpSFXNum;

    //Bunch of variables I want to see in editor but not change
    [SerializeField]
    private GameObject bottleThrown;
    [SerializeField]
    private Transform throwingArm;
    [SerializeField]
    private GameObject colRunning; //The running collider
    [SerializeField]
    private GameObject colSliding; //The sliding collider
    [SerializeField]
    private GameObject colBoozePower; //The booze power collider
    [SerializeField]
    private GameObject colPunch;
    [SerializeField]
    private PlatformMover[] startPlatforms; //All the platforms currently in the scene
    [SerializeField]
    private EnemyBehaviour[] enemies; //All the enemies currently in the scene
    [SerializeField]
    private BottlePickup[] bottleCollectibles;
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
        bottleCollectibles = GameObject.FindObjectsOfType<BottlePickup>();
        sfxManager = GameObject.FindObjectOfType<SFXManager>();
        jumpSFXNum = sfxManager.jumpSFX.Length;

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
        if (!isSliding)
        {
            isGrounded = Physics2D.IsTouchingLayers(runCollider, whatIsGround);
        }
        else
        {
            isGrounded = Physics2D.IsTouchingLayers(slideCollider, whatIsGround);
        }

        anim.SetBool("isGrounded", isGrounded); //Makes sure the animations go back to normal if the player touches the ground

        if (!gameStarted) //Checks to see if the game is set to "pause" mode
        {
            if (Input.GetKeyDown(KeyCode.Space)) //Change keycode to something more appropriate once game is near completion
            {
                StartRunning(); //Starts the game!
            }
        }
        else if (dead) //Checks to see if you died a horrible painful death
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetEverything(); //Three guesses as to what this does
            }
        }
        else if(!gameEnd)//This should be the default setting, when the game is running
        {
            Jump(); //Lets you soar through the air without a care in the world
            Slide(); //Lets you slide real smooth like
            ActivateBoozePower(); //Activates the wonderful magical power of BOOZE
            Throw();
            Punch();

            //The booze power timer
            if (boozedUp) //Are you drunk?
            {
                boozeTimer -= Time.deltaTime;

                if (boozeTimer <= 0)
                {
                    DeactivateBoozePower(); //You are now sober/sad
                }
            }

        }
        else
        {
            
        }

    }

    void JumpSFX()
    {
        int randoJump = Random.Range(0, jumpSFXNum);
        sfxManager.audioSource[0].clip = sfxManager.jumpSFX[randoJump];
        sfxManager.audioSource[0].Play();
    }

    public void BonkSFX()
    {
        sfxManager.audioSource[1].clip = sfxManager.bonkSFX;
        sfxManager.audioSource[1].Play();
    }

    void CheckPointSFX()
    {
        sfxManager.audioSource[2].clip = sfxManager.checkPointSFX;
        sfxManager.audioSource[2].Play();
    }

    public void SlideSFX()
    {
        sfxManager.audioSource[4].clip = sfxManager.slideSFX;
        sfxManager.audioSource[4].Play();
    }

    void PunchSFX()
    {
        sfxManager.audioSource[5].clip = sfxManager.punchSFX;
        sfxManager.audioSource[5].Play();
    }

    void DrinkSFX()
    {
        sfxManager.audioSource[6].clip = sfxManager.drinkSFX;
        sfxManager.audioSource[6].Play();
    }

    void ThrowSFX()
    {
        sfxManager.audioSource[7].clip = sfxManager.throwSFX;
        sfxManager.audioSource[7].Play();
    }

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(1f);

        readyThrow = true;
    }

    //This happens when you touch a thing
    public void Dead()
    {
        if(!dead)
        {
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
            DecreaseBottles();
        }
    }

    public void PunchEventStart()
    {
        colPunch.SetActive(true);
        isPunching = true;
    }

    public void PunchEventEnd()
    {
        colPunch.SetActive(false);
        isPunching = false;
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
        ShooterBehaviour shooter = col.gameObject.GetComponent<ShooterBehaviour>();
        BulletBehaviour bullet = col.gameObject.GetComponent<BulletBehaviour>();

        if (enemy || shooter || bullet) //Makes sure you don't die when you touch the ground
        {
            Dead();
        }
    }

    //The even more mighty trigger detector, if this hits something, a whole slew of things happen!
    void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyBehaviour enemy = collider.gameObject.GetComponent<EnemyBehaviour>();
        ShooterBehaviour shooter = collider.gameObject.GetComponent<ShooterBehaviour>();
        BulletBehaviour bullet = collider.gameObject.GetComponent<BulletBehaviour>();

        if ((enemy || bullet || shooter) && boozedUp) //Destroys ALL obstacles if you're boozed up
        {
            if(enemy)
            {
                enemy.DeathAnim();
            }
            else if(shooter)
            {
                shooter.DeathAnim();
            }

            collider.gameObject.SetActive(false);


        }
        else if(shooter && !boozedUp && isPunching) //Destroys only shooters when punching
        {
            shooter.DeathAnim();
            collider.gameObject.SetActive(false);

        }

        if(collider.tag == "PitStop") //Sets the checkpoint if a pitstop is touched
        {
            SetCheckPoint();
            CheckPointSFX();
        }

        if(collider.tag == "Booze") //adds a Bottle of booze if you touch one
        {
            IncreaseBottles();
            collider.gameObject.SetActive(false);
        }

        if(collider.tag == "End")
        {
            EndSequence();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(pitStopTouched)
        {
            pitStopTouched = false;
        }
    }

    void EndSequence()
    {
        anim.SetBool("isRunning", false);

        gameEnd = true;

        if(bottles < minBottleReq)
        {
            anim.SetTrigger("isSad");
        }
        else
        {
            anim.SetTrigger("isHappy");
        }

        foreach(PlatformMover plats in startPlatforms)
        {
            plats.PauseGame();
        }
        foreach(EnemyBehaviour bads in enemies)
        {
            bads.StopMoving();
        }

        //TODO: Put the Iris transition here
        //TODO: Call LevelManager to change the level
    }

    //Drink up, be merry, and also invincible for a couple of seconds (pits don't care if you're drunk)
    void ActivateBoozePower()
    {
        if(Input.GetButtonDown("BoozeUp"))
        {
            if(!boozedUp && bottles >= 5)
            {
                DrinkSFX();
                bottles -= 5;
                UpdateBottleCountDisplay();
                boozedUp = true;
                colBoozePower.SetActive(true);
            }
            else if(bottles < 5)
            {
                Debug.Log("Not enough booze!!");
            }
        }
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
        foreach (PlatformMover plats in startPlatforms)
        {
            plats.ResetPos();
            plats.gameObject.SetActive(true);
        }
        foreach (EnemyBehaviour bads in enemies)
        {
            bads.ResetPosition();
            bads.gameObject.SetActive(true);
        }

        foreach(BottlePickup collBottles in bottleCollectibles)
        {
            collBottles.gameObject.SetActive(true);
        }

        dead = false;
        Initialize();
    }

    //Changes the running collider to the slide collider, also makes you look real cool
    void Slide()
    {
        if (Input.GetButton("Slide"))
        {
            anim.SetBool("isSliding", true);
            colRunning.SetActive(false);
            colSliding.SetActive(true);
            isSliding = true;

        }
        else if (Input.GetButtonUp("Slide"))
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
        if (Input.GetButtonDown("Jump"))
        {
            //You can't jump while you're already jumping, dummy!
            if (isGrounded)
            {
                anim.SetTrigger("isJumping");
                JumpSFX();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
        if (bottles >= 5)
        {
            bottles -= 5;
            UpdateBottleCountDisplay();
        }
        else
        {
            bottles = 0;
            UpdateBottleCountDisplay();
        }
    }

    //Throws a bottle
    void Throw()
    {
        if (Input.GetButtonDown("Throw")) //Set to something better
        {
            if(!isSliding && readyThrow)
            {
                if(bottles > 0)
                {
                    anim.SetTrigger("isThrowing");
                    ThrowSFX();
                    GameObject bottleProj = Instantiate(bottleThrown, throwingArm.position, Quaternion.identity) as GameObject;
                    bottles--;
                    UpdateBottleCountDisplay();
                    readyThrow = false;
                    Destroy(bottleProj, 3f);
                    StartCoroutine(ResetThrow());
                }
                else
                {
                    Debug.Log("No more bottles!");
                }
            }
        }

    }

    //Punchs a human enemy (?)
    void Punch()
    {
        if (Input.GetButtonDown("Punch")) //Set to something better
        {
            anim.SetTrigger("isPunching");
            PunchSFX();
        }
    }

    //Updates your booze count, white text if in normal range, green if above 24 and red if below...5?
    void UpdateBottleCountDisplay()
    {
        if (bottles <= minBottleReq)
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

    void SetCheckPoint()
    {
        if(!pitStopTouched)
        {
            pitStopTouched = true;

            if(bottles >= 10)
            {
                foreach(PlatformMover plats in startPlatforms)
                {
                    Vector3 newPos = plats.GetPlatPosition();
                    plats.SetPlatPosition(newPos - new Vector3(7, 0, 0));
                }
                foreach(EnemyBehaviour bads in enemies)
                {
                    Vector3 newPos = bads.GetPosition();
                    bads.SetPosition(newPos);
                }
                bottles -= minBottleReq;
                UpdateBottleCountDisplay();
                Debug.Log("Reduced Bottles by " + minBottleReq);

            }
            else
            {
                Debug.Log("Not enough bottles for this checkpoint!");
            }
        }
    }

    //Sets all the colliders, animations, positions and conditions for the player back to normal.
    void Initialize()
    {
        colRunning.SetActive(true);
        colBoozePower.SetActive(false);
        colSliding.SetActive(false);
        colPunch.SetActive(false);
        gameStarted = false;
        gameEnd = false;
        boozedUp = false;
        dead = false;
        transform.position = initPos;

        anim.SetBool("isDead", false);
        anim.SetBool("isIdle", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isSliding", false);
    }
}
