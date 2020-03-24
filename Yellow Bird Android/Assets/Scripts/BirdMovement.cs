using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BirdMovement : MonoBehaviour
{
    public int birdNumber;

    public bool firstFlap;
    public bool gameWasInAlmostPlayModeBeforePausing;
    public bool gameWasInPlayModeBeforePausing;
    public bool gameWasInDeathModeBeforePausing;
    public float alphaOptionsBeforeFade;
    public float alphaPlayBeforeFade;
    public bool gameCanRestart;
    private int x;
    private int k;

    //public bool gameIsReadyToPlayAfterPauseIsOver;
    //public int pauseDelayInDots;
    public int tapPlace;

    public int countdownFromTap;
    public bool gameIsInMenuMode;
    public bool gameIsInPlayMode;
    public bool gameIsInGameOverMode;
    public bool gameIsInAlmostPlayMode;
    public bool gameIsInPausedMode;
    public bool gameIsInDeathMode;
    public BGSpawner _BGSpawner;
    public ButtonShareScript _ButtonShareScript;
    public Touch touch;
    public Vector2 mousePos;

    public GameOptions _GameOptions;
    public Vector3 menuGametypeFlapSpeed;
    public FadeOutScript _FadeOutScriptOptions;
    public FadeOutScript _FadeOutScriptPlayButton;
    public FadeOutScript _FadeOutScriptTapTapImage;
    public CameraTracksPlayer _Camera;
    public PanelNotificationsScript _PanelNotifications;
    public PanelOptionsScript _PanelOptions;
    public Fading _FadingToBlack;
    public GameObject musicPrefab;
    public ButtonAreYouSureScript _ButtonAreYouSureScript;

    //public GroundMover _BGSky;
    public Score _Score;

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
        gameCanRestart = true;
        //hideAlmostPlayObject ();
        //EnableSound();
        k = 0;
        alphaOptionsBeforeFade = 1.0f;
        alphaPlayBeforeFade = 1.0f;
        lastPosition = transform.position;
        x = 0;
        numberOfButtonPresses = 0;
        menuGametypeFlapSpeed.x -= forwardSpeed;
        _GameOptions.SkyTypeContinuous();
        _GameOptions.FlapTypeToTheRight();
        timeFromStartOfTheGame = 0;
        firstFlap = false;
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
            pos.x = 0f;
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
        velocity.x = forwardSpeed;
        frame = 3f;
        showPaused();
        hideGameOver();
        gameStarted = false;
        dead = false;
        //if(Application.loadedLevel==0){birdAtMenu=true;animator.SetTrigger("FlapAtMenu");gameStarted=false;}

        //hideAlmostPlayObject ();
        _FadeOutScriptTapTapImage.FadeOutObject(0.0f, 0.0f);

        if (PlayerPrefs.GetInt("gameType") == 2)
        {
            StartTheGameForcefully();
            //Debug.Log ("da");
            _GameOptions.GameTypeOne();
        }

        //QualitySettings.vSyncCount = 0;
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
        MoveSky();
        tapPlace = 2;

        //        if (Input.GetMouseButtonDown(0))
        //        {
        //            mousePos = Input.mousePosition;
        //        }

        if (ClickedOrPressed())
        {
            mousePos = Input.mousePosition;
            if (mousePos.y < (Screen.height * 0.85))
            {
                tapPlace = 1;
            }
        }

        if (countdownFromTap > 0)
        {
            countdownFromTap--;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && gameIsInPausedMode)
            _PanelOptions.SlideOutPanelOptions();
        else Application.Quit();

        if (_Score.ScoreValue() == 1 && gameIsInPlayMode)
        {
            showScoreObject();
        }

        if (tapPlace == 1 && gameIsInAlmostPlayMode && !EventSystem.current.IsPointerOverGameObject())
        {
            MakeGameBeInPlayMode();
            _FadeOutScriptTapTapImage.FadeOutObject(0.0f, 0.6f);

            if (GameObject.Find("Pipes(Clone)") == null)
            {
                _BGSpawner.InitialisePipes();
            }
        }

        //Debug.Log ("numberofbuttonpresses: " + numberOfButtonPresses);
        if (timeFromStartOfTheGame < 1000)
        {
            timeFromStartOfTheGame++;
            if (timeFromStartOfTheGame == 35 && gameIsInMenuMode)
            {
                _FadeOutScriptOptions.FadeInObject(1.0f, 1.0f);
                alphaOptionsBeforeFade = 1.0f;
            }
            else if (timeFromStartOfTheGame == 25 && gameIsInMenuMode)
            {
                _FadeOutScriptPlayButton.FadeInObject(1f, 0.75f);
                alphaPlayBeforeFade = 1.0f;
            }
        }
        ///////////////////////////////////////////

        if ((gameIsInMenuMode || gameIsInAlmostPlayMode) && firstFlap == false)
        {
            CheckIfFlapped();
            transform.position += menuGametypeFlapSpeed;
        }
        else if (gameIsInAlmostPlayMode && tapPlace == 1)
        {
            numberOfButtonPresses++;
            if (numberOfButtonPresses == 1)
            {
                _BGSpawner.InitialisePipes();
                MakeGameBeInPlayMode();
            }
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
            showGameOver();
        }
        else if (ClickedOrPressed() && timeToIgnoreInput == 0 && (gameIsInPlayMode || gameIsInAlmostPlayMode) &&
                 !EventSystem.current.IsPointerOverGameObject() && countdownFromTap == 0)
        {
            audioS.clip = flapClip;
            countdownFromTap = 6;
            gameStarted = true;
            didFlap = true;
            CheckIfFlapped();
            hidePaused();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (birdAtMenu == false)
        {
            animator.SetTrigger("Death");
            gameWasInDeathModeBeforePausing = true;
            gameWasInAlmostPlayModeBeforePausing = false;
            gameWasInPlayModeBeforePausing = false;
            dead = true;
            deathCooldown = 0.4f;
            if (!gameIsInDeathMode)
            {
                MakeGameBeInDeathMode();
            }

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
            velocity.x = forwardSpeed;
            velocity.y -= gravity;
        }
    }

    private void CheckIfDead()
    {
        if (dead)
        {
            deathCooldown -= Time.deltaTime;
            if (gameCanRestart && deathCooldown <= 0 && ClickedOrPressed() &&
                !EventSystem.current.IsPointerOverGameObject() && countdownFromTap == 0 && tapPlace == 1 &&
                _PanelOptions.PanelOptionsIsDown == false)
            {
                countdownFromTap = 6;
                audioTimesPlayed = 0;
                //MakeGameBeInMenuMode ();
                PlayerPrefs.Save();
                _GameOptions.GameTypeTwo();
                Application.LoadLevel("SceneGame");
                MakeGameBeInMenuMode();
                _Score.SetScoreValue(0);
                //MakeGameBeInMenuMode ();
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
            _ButtonAreYouSureScript.MakeButtonInvisibleAndNoninteractable();

            if (_Score.ScoreValue() > PlayerPrefs.GetInt("highScore", 0))
            {
                _PanelNotifications.SlideInPanelNotificationsForcefully(
                    "Nice, you set a new personal record" + "\n" + "New High Score: " + _Score.ScoreValue(), 20);
                _ButtonShareScript.MakeButtonVisibleAndInteractable();
            }
            else if (_Score.ScoreValue() < PlayerPrefs.GetInt("highScore", 0))
            {
                _PanelNotifications.SlideInPanelNotificationsForcefully(
                    "YOU DIED" + "\n" + "Your score: " + _Score.ScoreValue() + "\n" + "High Score: " +
                    PlayerPrefs.GetInt("highScore", 0), 20);
            }
            else if (_Score.ScoreValue() == PlayerPrefs.GetInt("highScore", 0))
            {
                _PanelNotifications.SlideInPanelNotificationsForcefully(
                    "You matched your high score" + "\n" + "Your score: " + _Score.ScoreValue() + "\n" +
                    "High Score: " + PlayerPrefs.GetInt("highScore", 0), 20);
            }
        }

        k = 1;

        //artificialGravity = false;
        //velocity.x = 0;
        //velocity.y = 0;

        //forwardSpeed = 0;
        rb.gravityScale = 1;

        rb.drag = 2f;
        rb.angularDrag = 2f;
        //if (transform.localPosition.y <= 1)
        //gravity = 0;
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
            firstFlap = true;
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

    public IEnumerator StartGame()
    {
        if (timeFromStartOfTheGame >= 27)
        {
            EnableSound();

            _FadeOutScriptPlayButton.FadeOutAndMakeNonInteractable(0.0f, 0.5f);
            alphaPlayBeforeFade = 0.0f;
            _FadeOutScriptOptions.FadeOutObject(0.33f, 1.0f);
            alphaOptionsBeforeFade = 0.33f;
            yield return new WaitForSeconds(0.3f);
            _FadingToBlack.BeginFade(1);
            yield return new WaitForSeconds(0.65f);
            x++;
            //Debug.Log ("game started");
            _FadeOutScriptTapTapImage.FadeInObject(1.0f, 1.0f);
            //showAlmostPlayObject ();
            if (x == 1)
            {
                _Camera.MoveCameraInPlayPosition();
            }

            _FadingToBlack.BeginFade(-1);
            yield return new WaitForSeconds(0.2f);

            //menuGametypeFlapSpeed.x -= forwardSpeed;

            numberOfButtonPresses++;
            MakeGameBeInAlmostPlayMode();

            //if(_Score.score==1)
            //if(_Score.ScoreValue==1)showScoreObject ();
            //hideMenuObject ();
            //_PanelMenu.SlideOutPanelMenu();

            //_FadeOutScriptPlayButton.FadeOutAndMakeNonInteractable (0.0f, 1.0f);
            //alphaPlayBeforeFade = 0.0f;
            //_FadeOutScriptOptions.FadeOutObject (0.33f, 1.0f);
            //alphaOptionsBeforeFade = 0.33f;

            //BGSpawner.InitialisePipes ();

            yield return null;
        }
    }

    public void StartTheGame()
    {
        StartCoroutine("StartGame");
    }

    public void EnableSound()
    {
        PlayerPrefs.SetInt("soundTypeEnabled", 1);
        PlayerPrefs.SetInt("soundTypeDisabled", 0);
        //Debug.Log("Sound type set to enabled");
        GameObject musicGo = GameObject.FindGameObjectWithTag("Music");
        if (musicGo == null)
        {
            //Debug.Log("Couldn't find an object with tag music!");
            GameObject newActor = Instantiate(musicPrefab) as GameObject;
        }

        InitialiseSoundAndVibration();
        PlayerPrefs.Save();
    }

    public IEnumerator StartGameForcefully()
    {
        //hideMenuObject ();
        MakeGameBeInAlmostPlayMode();
        _FadeOutScriptPlayButton.FadeOutAndMakeNonInteractable(0.0f, 0.1f);
        alphaPlayBeforeFade = 0.0f;
        //_FadeOutScriptOptions.FadeOutObject (0.33f, 1.0f);
        alphaOptionsBeforeFade = 0.33f;
        //yield return new WaitForSeconds (0.3f);
        _FadingToBlack.BeginFade(1);
        //yield return new WaitForSeconds (0.65f);
        yield return new WaitForSeconds(0.3f);
        x++;
        //Debug.Log ("game started");
        //showAlmostPlayObject ();
        _FadeOutScriptTapTapImage.FadeInObject(1.0f, 1.0f);
        if (x == 1)
        {
            _Camera.MoveCameraInPlayPosition();
        }

        _FadingToBlack.BeginFade(-1);
        yield return new WaitForSeconds(0.2f);

        numberOfButtonPresses++;
        MakeGameBeInAlmostPlayMode();

        //if(_Score.score==1)
        //if(_Score.ScoreValue==1)showScoreObject ();
        //hideMenuObject ();
        //_PanelMenu.SlideOutPanelMenu();

        //_FadeOutScriptPlayButton.FadeOutAndMakeNonInteractable (0.0f, 1.0f);
        //alphaPlayBeforeFade = 0.0f;
        _FadeOutScriptOptions.FadeInObject(0.33f, 1.0f);
        //alphaOptionsBeforeFade = 0.33f;

        //BGSpawner.InitialisePipes ();

        yield return null;
    }

    public void StartTheGameForcefully()
    {
        StartCoroutine("StartGameForcefully");
    }

    public void MakeGameBeInPausedMode()
    {
        gameIsInPausedMode = true;
        gameIsInMenuMode = false;
        gameIsInAlmostPlayMode = false;
        gameIsInGameOverMode = false;
        gameIsInPlayMode = false;
        //Debug.Log ("Game is in Options Mode!");
    }

    public void MakeGameBeInAlmostPlayMode()
    {
        gameIsInPausedMode = false;
        gameIsInMenuMode = false;
        gameIsInAlmostPlayMode = true;
        gameIsInGameOverMode = false;
        gameIsInPlayMode = false;
        gameIsInDeathMode = false;
        gameWasInAlmostPlayModeBeforePausing = true;
        gameWasInPlayModeBeforePausing = false;
        //Debug.Log ("Game is in Almostplay Mode!");
    }

    public void MakeGameBeInMenuMode()
    {
        gameIsInPausedMode = false;
        gameIsInMenuMode = true;
        gameIsInAlmostPlayMode = false;
        gameIsInGameOverMode = false;
        gameIsInPlayMode = false;
        gameIsInDeathMode = false;
        //Debug.Log ("Game is in Menu Mode!");
    }

    public void MakeGameBeInPlayMode()
    {
        gameIsInPausedMode = false;
        gameIsInMenuMode = false;
        gameIsInAlmostPlayMode = false;
        gameIsInGameOverMode = false;
        gameIsInPlayMode = true;
        gameIsInDeathMode = false;
        gameWasInAlmostPlayModeBeforePausing = false;
        gameWasInPlayModeBeforePausing = true;
        //Debug.Log ("Game is in Play Mode!");
    }

    public void MakeGameBeInDeathMode()
    {
        gameIsInPausedMode = false;
        gameIsInMenuMode = false;
        gameIsInAlmostPlayMode = false;
        gameIsInGameOverMode = false;
        gameIsInPlayMode = false;
        gameIsInDeathMode = true;
        gameWasInAlmostPlayModeBeforePausing = false;
        gameWasInPlayModeBeforePausing = true;

        //Debug.Log ("Game is in Death Mode!");
    }

    public void showPaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    public void hidePaused()
    {
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void showGameOver()
    {
        foreach (GameObject g in gameOverObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideGameOver()
    {
        foreach (GameObject g in gameOverObjects)
        {
            g.SetActive(false);
        }
    }

    public void showScoreObject()
    {
        foreach (GameObject g in scoreObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideScoreObject()
    {
        foreach (GameObject g in scoreObjects)
        {
            g.SetActive(false);
        }
    }

    public void showMenuObject()
    {
        foreach (GameObject g in menuObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideMenuObject()
    {
        foreach (GameObject g in menuObjects)
        {
            g.SetActive(false);
        }
    }

    public void showAlmostPlayObject()
    {
        foreach (GameObject g in almostPlayObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideAlmostPlayObject()
    {
        foreach (GameObject g in almostPlayObjects)
        {
            g.SetActive(false);
        }
    }

    /*public void MoveSky ()
	{
		GameObject[] skies = GameObject.FindGameObjectsWithTag ("Sky");
		if (gameIsInMenuMode || gameIsInAlmostPlayMode) {
			foreach (GameObject sky in skies) {
				Vector3 pos = sky.transform.position;
				pos.x += 0.01f;
				sky.transform.position = pos;
			}
		} else if (gameIsInPlayMode) {
			foreach (GameObject sky in skies) {
				Vector3 pos = sky.transform.position;
				pos.x += 0.02f;
				sky.transform.position = pos;
			}
		}
	}*/

    public void MoveSky()
    {
        GameObject[] skies = GameObject.FindGameObjectsWithTag("Sky");
        if (gameIsInMenuMode)
        {
            foreach (GameObject sky in skies)
            {
                Vector3 pos = sky.transform.position;
                pos.x += 0.01f;
                sky.transform.position = pos;
            }
        }
        else if (gameIsInPlayMode)
        {
            foreach (GameObject sky in skies)
            {
                Vector3 pos = sky.transform.position;
                pos.x += 0.02f;
                sky.transform.position = pos;
            }
        }
        else if (gameIsInAlmostPlayMode)
        {
            //do nothing
        }
    }
}