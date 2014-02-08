using UnityEngine;
using System.Collections;

public class GameOverGUIScript : MonoBehaviour {

	public GUISkin		fontSkin;
	public AudioClip	gameOver;
	private bool		hasPlayed = false;

	void OnGUI () {
		GUI.skin = fontSkin;
		GUI.Label (new Rect (140, 150, 200, 100), "GAME OVER");

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
