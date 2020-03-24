using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Player2 : MonoBehaviour
{
    public int birdNumber;

    public BirdMovement bird0;

    private int x;
    private int k;

    public int tapPlace;

    public int countdownFromTap;
    public bool gameIsInMenuMode;
    public bool gameIsInPlayMode;
    public bool gameIsInGameOverMode;
    public bool gameIsInAlmostPlayMode;
    public bool gameIsInPausedMode;
    public bool gameIsInDeathMode;

    public Vector3 menuGametypeFlapSpeed;

    public int numberOfButtonPresses;

    public int timeFromStartOfTheGame;
    private Animator animator;
    public static bool dead = false;
    private bool didFlap = false;
    private float deathCooldown;
    public float forwardSpeed;
    public float flapSpeed;
    public float gravity;
    public float upperLimit;
    public Vector3 velocity;
    public Rigidbody2D rb;
    private bool artificialGravity = true;
    private float frame;
    public static bool gameStarted = false;
    private int toggle;
    private Quaternion rotation;
    private Vector3 pos;
    private AudioSource audioS;
    public AudioClip hitClip;
    public AudioClip flapClip;

    //bool dead = false;
    private bool birdAtMenu = false;

    private int audioTimesPlayed;
    private GameObject[] pauseObjects;
    private GameObject[] gameOverObjects;
    private GameObject[] scoreObjects;
    private GameObject[] menuObjects;
    private GameObject[] almostPlayObjects;
    private GameObject spawnerObject;
    private bool canPlaySound;
    private bool canPlayVibration;

    private int timeToIgnoreInput;

    private int s = 1;

    private Vector3 lastPosition;
    //0.026,0.07,0.0045

    private void Start()
    {
        //gameIsReadyToPlayAfterPauseIsOver = false;
        //hideAlmostPlayObject ();
        //EnableSound();
        k = 0;
        lastPosition = bird0.transform.position;
        lastPosition.x -= 0.5f;
        x = 0;
        numberOfButtonPresses = 0;
        //        menuGametypeFlapSpeed.x -= forwardSpeed;
        menuGametypeFlapSpeed = bird0.menuGametypeFlapSpeed;

        timeFromStartOfTheGame = 0;
        spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        gameOverObjects = GameObject.FindGameObjectsWithTag("ShowOnGameOver");
        scoreObjects = GameObject.FindGameObjectsWithTag("ScoreObject");
        menuObjects = GameObject.FindGameObjectsWithTag("ShowOnMenu");
        almostPlayObjects = GameObject.FindGameObjectsWithTag("ShowOnAlmostPlay");
        gameIsInMenuMode = true;
        if (PlayerPrefs.GetInt("soundTypeEnabled") == 1)
        {
            canPlaySound = true;
        }
        else
            canPlaySound = false;

        if (PlayerPrefs.GetInt("vibrationTypeEnabled") == 1)
        {
            canPlayVibration = true;
        }
        else
            canPlayVibration = false;

        audioTimesPlayed = 0;
        audioS = GetComponent<AudioSource>();
        audioS.clip = flapClip;

        toggle = 1;
        if (PlayerPrefs.GetInt("flapTypeToTheRight") == 1 && Application.loadedLevel == 0)
        {
            toggle = 1;
            rotation.Set(0, 0, 0, 0);
            pos = this.transform.localPosition;
            pos.x = -0.5f;
            this.transform.localPosition = pos;
        }
        else if (PlayerPrefs.GetInt("flapTypeToTheLeft") == 1 && Application.loadedLevel == 0)
        {
            toggle = -1;
            rotation.Set(0, 180, 0, 0);
            pos = this.transform.localPosition;
            pos.x = 0.8f;
            this.transform.localPosition = pos;
        }

        forwardSpeed *= toggle;

        this.transform.localRotation = rotation;

        rb = GetComponent<Rigidbody2D>();
        InitialiseAnimator();
        velocity.x = bird0.forwardSpeed;
        frame = 3f;
        gameStarted = false;
        dead = false;

        Application.targetFrameRate = 60;
    }

    private bool ClickedOrPressed()
    {
        if (Input.GetMouseButtonDown(0) && birdNumber == 0) return true;
        if (Input.GetKeyDown(KeyCode.Space) && birdNumber == 1) return true;

        return false;
    }

    private void Update()
    {
        gameIsInAlmostPlayMode = bird0.gameIsInAlmostPlayMode;
        gameIsInDeathMode = bird0.gameIsInDeathMode;
        gameIsInGameOverMode = bird0.gameIsInGameOverMode;
        gameIsInMenuMode = bird0.gameIsInMenuMode;
        gameIsInPausedMode = bird0.gameIsInPausedMode;
        gameIsInPlayMode = bird0.gameIsInPlayMode;

        tapPlace = 2;

        if (ClickedOrPressed())
        {
            tapPlace = 1;
        }

        if (countdownFromTap > 0)
        {
            countdownFromTap--;
        }

        if ((gameIsInMenuMode || gameIsInAlmostPlayMode) && bird0.firstFlap == false)
        {
            CheckIfFlapped();
            menuGametypeFlapSpeed.x = bird0.menuGametypeFlapSpeed.x;
            transform.position += menuGametypeFlapSpeed;
        }

        timeToIgnoreInput = TimerManager.timeToIgnoreInput;
        if (timeToIgnoreInput > 0)
        {
            timeToIgnoreInput -= 1;
        }

        if (gameStarted && !dead && gameIsInPlayMode)
        {
            frame += 0.15f;
            if (countdownFromTap == 0)
            {
                CheckIfDead();
            }

            SetVelocity();
            CheckIfFlapped();
            SetAngle();
        }
        else if (dead && countdownFromTap == 0)
        {
            CheckIfDead();
            Die();
        }
        else if (ClickedOrPressed() && timeToIgnoreInput == 0 && (gameIsInPlayMode || gameIsInAlmostPlayMode) && countdownFromTap == 0)
        {
            audioS.clip = flapClip;
            countdownFromTap = 6;
            gameStarted = true;
            didFlap = true;
            CheckIfFlapped();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (birdAtMenu == false)
        {
            animator.SetTrigger("Death");

            dead = true;
            deathCooldown = 0.4f;

            Die();
        }

        if (audioTimesPlayed <= 2)
        {
            audioS.clip = hitClip;
            if (canPlaySound)
            {
                audioS.Play();
            }

            if (PlayerPrefs.GetInt("vibrationTypeEnabled") == 1)
            {
                canPlayVibration = true;
            }
            else
                canPlayVibration = false;

            if (canPlayVibration)
            {
                //Handheld.Vibrate();
                //////////////////////////////////////////////////////////////////////////////
            }

            audioTimesPlayed++;
        }
    }

    private void SetVelocity()
    {
        if (artificialGravity)
        {
            velocity.x = bird0.forwardSpeed;
            velocity.y -= bird0.gravity;
        }
    }

    private void CheckIfDead()
    {
        if (dead)
        {
            deathCooldown -= Time.deltaTime;
            if (bird0.gameCanRestart && deathCooldown <= 0 && ClickedOrPressed() &&
                !EventSystem.current.IsPointerOverGameObject() && countdownFromTap == 0 && tapPlace == 1)
            {
                countdownFromTap = 6;
                audioTimesPlayed = 0;
                PlayerPrefs.Save();
                Application.LoadLevel("SceneGame");
            }
        }
        else if (ClickedOrPressed() && countdownFromTap == 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            countdownFromTap = 6;
            didFlap = true;
        }
    }

    private void Die()
    {
        if (k == 0)
        {
            rb.AddForce(transform.up * 0.5f);
            rb.AddForce(transform.right * 1f);
        }

        k = 1;
        rb.gravityScale = 1;

        rb.drag = 2f;
        rb.angularDrag = 2f;
    }

    private void SetAngle()
    {
        if (artificialGravity)
        {
            float angle = Mathf.Lerp(45, -75, frame / 7f);

            if (toggle == 1)
            {
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else if (toggle == -1)
            {
                transform.rotation = Quaternion.Euler(0, 180, angle);
            }
        }
    }

    private void CheckIfFlapped()
    {
        if (tapPlace == 1 && didFlap == true && transform.position.y < upperLimit && artificialGravity && dead == false)
        {
            didFlap = false;
            bird0.firstFlap = true;
            frame = 0;
            animator.SetTrigger("DoFlap");

            velocity.y = flapSpeed;
            if (canPlaySound && Application.loadedLevel == 0)
                audioS.Play();
        }

        velocity.x = bird0.velocity.x;
        transform.position += velocity;
    }

    private void InitialiseAnimator()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
        }
    }

    public void InitialiseSoundAndVibration()
    {
        if (PlayerPrefs.GetInt("soundTypeEnabled") == 1)
        {
            canPlaySound = true;
        }
        else
            canPlaySound = false;

        if (PlayerPrefs.GetInt("vibrationTypeEnabled") == 1)
        {
            canPlayVibration = true;
        }
        else
            canPlayVibration = false;
    }

    public void EnableSound()
    {
        PlayerPrefs.SetInt("soundTypeEnabled", 1);
        PlayerPrefs.SetInt("soundTypeDisabled", 0);
        GameObject musicGo = GameObject.FindGameObjectWithTag("Music");
        if (musicGo == null)
        {
            //            GameObject newActor = Instantiate(musicPrefab) as GameObject;
        }

        InitialiseSoundAndVibration();
        PlayerPrefs.Save();
    }
}