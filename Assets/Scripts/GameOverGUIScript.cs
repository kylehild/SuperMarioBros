using UnityEngine;
using System.Collections;

public class GameOverGUIScript : MonoBehaviour {
	
	public GUISkin		fontSkin;
	public AudioClip	gameOver;
	private bool		hasPlayed = false;
	
	private int		desiredWidth = 360;
	private int		desiredHeight = 315;
	private float	rW, rH;
	
	void OnGUI () {
		rW = (float) Screen.width / (float) desiredWidth;
		rH = (float) Screen.height / (float) desiredHeight;
		
		GUI.skin = fontSkin;
		GUI.Label (new Rect (rW*140, rH*150, 200, 100), "GAME OVER");
		
		if(!hasPlayed){
			audio.PlayOneShot (gameOver);
			hasPlayed = true;
		}
		Invoke ("GoToStart", 4f);
	}
	
	public void GoToStart(){
		Application.LoadLevel ("StartScreen");
	}
}

