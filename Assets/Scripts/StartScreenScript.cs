using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour {

	public GUISkin		fontSkin;
	public GameObject	Mario;
	
	private int			desiredWidth = 360;
	private int			desiredHeight = 315;
	private float		rW, rH;
	
	void OnGUI () {
		rW = (float) Screen.width / (float) desiredWidth;
		rH = (float) Screen.height / (float) desiredHeight;

		GUI.skin = fontSkin;
		if(GUI.Button (new Rect (rW*110, rH*210, rW*140, rH*20), "Play Level 1-1")){
			Mario.GetComponent<MarioControllerScript> ().initVariables ();
			Mario.GetComponent<MarioControllerScript>().setLastLevel("Level_1_1");
			Application.LoadLevel("LivesScreen");
		}
		else if(GUI.Button (new Rect (rW*55, rH*180, rW*245, rH*20), "Play Ryan and Kyle's Level")){
			Mario.GetComponent<MarioControllerScript> ().initVariables ();
			Mario.GetComponent<MarioControllerScript>().setLastLevel("Level_R_K"); //change this to original level
			Application.LoadLevel("LivesScreen");
		}
	}
}
