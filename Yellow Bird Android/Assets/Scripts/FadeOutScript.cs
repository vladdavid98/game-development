using UnityEngine;
using System.Collections;

public class FadeOutScript : MonoBehaviour {
	public CanvasGroup yourNameHere;
	float x,s;
	void Start()
	{
		CanvasGroup yourNameHere = GetComponent<CanvasGroup>();
	}
	



	IEnumerator FadeOut (){
		while(yourNameHere.alpha > x){                   //use "< 1" when fading in
			yourNameHere.alpha -= Time.deltaTime/s;    //fades out over 1 second. change to += to fade in    
			yield return null;
		}
	}
	IEnumerator FadeOutAndMakeNonInteractable (){
		while(yourNameHere.alpha > x){                   //use "< 1" when fading in
			yourNameHere.alpha -= Time.deltaTime/s;    //fades out over 1 second. change to += to fade in    
			yield return null;
		}
		yourNameHere.blocksRaycasts = false;
		//Debug.Log ("button no longer blocks raycasts");

	}
	IEnumerator FadeIn (){
		while(yourNameHere.alpha < x){                   //use "< 1" when fading in
			yourNameHere.alpha += Time.deltaTime/s;    //fades out over 1 second. change to += to fade in    
			yield return null;
		}
	}
	public void FadeOutObject(float a,float b)
	{
		x = a;
		s = b;
		StartCoroutine("FadeOut");
		//Debug.Log ("fadeout initiated");
	}
	public void FadeOutAndMakeNonInteractable(float a,float b)
	{
		x = a;
		s = b;
		StartCoroutine("FadeOutAndMakeNonInteractable");
	}
	public void FadeInObject(float a,float b)
	{
		x = a;
		s = b;
		StartCoroutine("FadeIn");
		//Debug.Log ("fadein initiated");
	}
}
