using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	private static Text text;
	private static Text highScoreText;

	private static int highScore;
	public static int score;

	void Awake ()
	{
		
		text = GetComponent<Text>();
	}

	void Start ()
	{
		score = 0;
		highScore = PlayerPrefs.GetInt("highScore",0);
		//text.text = "" + score;
		//+ "\n"+highScore; 
	}

	void OnDestroy ()
	{
		PlayerPrefs.SetInt("highScore",highScore);
		PlayerPrefs.Save ();
	}

	public static void Increment () {

		score++;
		//Debug.Log (score);
		if(score>=highScore)highScore=score;
		text.text = ""+score; 
		//Debug.Log ("Score: " + score);
		//highScoreText.text="high Score: " + highScore;
	}
	public int ScoreValue()
	{
		return score;
	}
	public void SetScoreValue(int x)
	{
		score = x;
	}
//	public void IncrementAgain()
//	{
//		score++;
//	}
	public void SetHighScoreToNull()
	{
		highScore = 0;
	}
	//public void 
}
