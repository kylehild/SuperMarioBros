using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour {

	public GUISkin		fontSkin;
	public GameObject	Mario;
	
	void OnGUI () {
		GUI.skin = fontSkin;
		if(GUI.Button (new Rect (110, 210, 130, 20), "Play Level 1-1")){
			Mario.GetComponent<MarioControllerScript> ().initVariables ();
			Mario.GetComponent<MarioControllerScript>().setLastLevel("Level_1_1");
			Application.LoadLevel("LivesScreen");
		}
		else if(GUI.Button (new Rect (65, 180, 220, 20), "Play Ryan and Kyle's Level")){
			Mario.GetComponent<MarioControllerScript> ().initVariables ();
			Mario.GetComponent<MarioControllerScript>().setLastLevel("Level_1_1"); //change this to original level
			Application.LoadLevel("LivesScreen");
		}
	}
}
