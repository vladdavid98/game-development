using UnityEngine;
using System.Collections;

public class CameraTracksPlayer : MonoBehaviour
{
    private Transform player;
    private float offsetX;
    public BirdMovement _Player;
    private bool dead;

    // Use this for initialization
    private void Start()
    {
        dead = false;
        GameObject player_go = GameObject.FindGameObjectWithTag("Player");
        if (player_go == null)
        {
            return;
        }
        if (PlayerPrefs.GetInt("flapTypeToTheLeft") == 1 && Application.loadedLevel == 2)
        {
            Vector3 pos = transform.position;
            pos.x = -0.2f;
            transform.position = pos;
        }

        player = player_go.transform;

        offsetX = transform.position.x - player.position.x;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_Player.gameIsInDeathMode)
            dead = true;
        if (player != null && !dead)
        {
            Vector3 pos = transform.position;
            pos.x = player.position.x + offsetX;
            transform.position = pos;
        }
    }

    public void MoveCameraInPlayPosition()
    {
        offsetX++;
    }
}