using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour
{
    public GameObject musicPrefab;
    public BirdMovement _BirdMovement;
//    public Score _Score;
    public PanelNotificationsScript _PanelNotifications;
    public ButtonAreYouSureScript _ButtonAreYouSureScript;
    private int type;

    private void Start()
    {
        type = PlayerPrefs.GetInt("skyType");
    }

    public IEnumerator ResetScore()
    {
        _ButtonAreYouSureScript.MakeButtonVisibleAndInteractable();
        _PanelNotifications.SlideInPanelNotifications("Are you absolutely sure you want to" + "\n" + "reset all your game data?", 4.0f);
        yield return new WaitForSeconds(7.45f);
        _ButtonAreYouSureScript.MakeButtonInvisibleAndNoninteractable();
        yield return null;
    }

    public void ResetScorez()
    {
        if (!_PanelNotifications.slidin)
        {
            StartCoroutine("ResetScore");
        }
    }

    public void SkyTypeContinuous()
    {
        PlayerPrefs.SetInt("skyTypeContinuous", 1);
        PlayerPrefs.SetInt("skyTypeStatic", 0);
    }

    public void SkyTypeStatic()
    {
        PlayerPrefs.SetInt("skyTypeContinuous", 0);
        PlayerPrefs.SetInt("skyTypeStatic", 1);
    }

    public void FlapTypeToTheRight()
    {
        PlayerPrefs.SetInt("flapTypeToTheRight", 1);
        PlayerPrefs.SetInt("flapTypeToTheLeft", 0);
    }

    public void FlapTypeToTheLeft()
    {
        PlayerPrefs.SetInt("flapTypeToTheRight", 0);
        PlayerPrefs.SetInt("flapTypeToTheLeft", 1);
    }

    public void SoundTypeEnabled()
    {
        if (!_PanelNotifications.slidin)
        {
            _PanelNotifications.SlideInPanelNotifications("Sound Enabled", 1.25f);
        }
        PlayerPrefs.SetInt("soundTypeEnabled", 1);
        PlayerPrefs.SetInt("soundTypeDisabled", 0);
        GameObject music_go = GameObject.FindGameObjectWithTag("Music");
        if (music_go == null)
        {
            GameObject newActor = Instantiate(musicPrefab) as GameObject;
        }
        _BirdMovement.InitialiseSound();
        PlayerPrefs.Save();
    }

    public void SoundTypeDisabled()
    {
        if (!_PanelNotifications.slidin)
        {
            _PanelNotifications.SlideInPanelNotifications("Sound Disabled", 1.25f);
        }
        PlayerPrefs.SetInt("soundTypeEnabled", 0);
        PlayerPrefs.SetInt("soundTypeDisabled", 1);
        GameObject music = GameObject.FindGameObjectWithTag("Music");
        Destroy(music);
        _BirdMovement.InitialiseSound();
        PlayerPrefs.Save();
    }

    public void GameTypeOne()
    {
        PlayerPrefs.SetInt("gameType", 1);
    }

    public void GameTypeTwo()
    {
        PlayerPrefs.SetInt("gameType", 2);
    }
}