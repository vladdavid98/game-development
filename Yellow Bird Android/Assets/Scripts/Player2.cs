using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Player2 : MonoBehaviour
{
    public int birdNumber;

    public BirdMovement bird0;

    public Score _Score;

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
    public bool dead = false;
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

    private Quaternion rotation;
    private Vector3 pos;
    private AudioSource audioS;
    public AudioClip hitClip;
    public AudioClip flapClip;

    private bool birdAtMenu = false;

    private int audioTimesPlayed;

    private bool canPlaySound;
    private int timeToIgnoreInput;

    private Vector3 lastPosition;

    private void Start()
    {
        bird0.player2alive = true;
        k = 0;
        lastPosition = bird0.transform.position;
        lastPosition.x -= 0.5f;
        x = 0;
        numberOfButtonPresses = 0;

        menuGametypeFlapSpeed = bird0.menuGametypeFlapSpeed;

        timeFromStartOfTheGame = 0;

        gameIsInMenuMode = true;
        if (PlayerPrefs.GetInt("soundTypeEnabled") == 1)
        {
            canPlaySound = true;
        }
        else
            canPlaySound = false;

        audioTimesPlayed = 0;
        audioS = GetComponent<AudioSource>();
        audioS.clip = flapClip;

        rotation.Set(0, 0, 0, 0);
        pos = this.transform.localPosition;
        pos.x = -0.5f;
        this.transform.localPosition = pos;

        this.transform.localRotation = rotation;

        rb = GetComponent<Rigidbody2D>();
        InitialiseAnimator();
        velocity.x = bird0.forwardSpeed;

        frame = 3f;
        gameStarted = false;
        dead = false;

        Application.targetFrameRate = 60;

        UpdatePosX(bird0);

        _Score = bird0._Score;
    }

    private void UpdatePosX(BirdMovement bird0)
    {
        if (!bird0.dead)
            transform.position = new Vector3(bird0.transform.position.x, transform.position.y, transform.position.z);
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    private bool ClickedOrPressed()
    {
        if (Input.GetMouseButtonDown(0) && birdNumber == 0) return true;
        if (Input.GetKeyDown(KeyCode.Space) && birdNumber == 1) return true;

        return false;
    }

    private void Update()
    {
        if (!dead)
            UpdatePosX(bird0);
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

            if (bird0.dead) bird0.MakeGameBeInDeathMode();

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

            audioTimesPlayed++;
        }
    }

    private void SetVelocity()
    {
        if (!artificialGravity) return;
        velocity.x = 0.026f;
        velocity.y -= 0.0045f;
    }

    private void CheckIfDead()
    {
        if (dead)
        {
            bird0.player2alive = false;
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

            transform.rotation = Quaternion.Euler(0, 0, angle);
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
        transform.position += velocity;
    }

    private void InitialiseAnimator()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
        }
    }

    public void InitialiseSound()
    {
        if (PlayerPrefs.GetInt("soundTypeEnabled") == 1)
        {
            canPlaySound = true;
        }
        else
            canPlaySound = false;
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

        InitialiseSound();
        PlayerPrefs.Save();
    }
}