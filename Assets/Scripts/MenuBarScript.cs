using UnityEngine;
using System.Collections;

public class MenuBarScript : MonoBehaviour {

	public GUISkin		fontSkin;
	public GameObject	Mario;
	public string		levelName = "1-1";
	
	private int			desiredWidth = 360;
	private int			desiredHeight = 315;
	private float		rW, rH;

	void OnGUI () {

		rW = (float) Screen.width / (float) desiredWidth;
		rH = (float) Screen.height / (float) desiredHeight;

		GUI.skin = fontSkin;
		GUI.Label (new Rect (rW*40, rH*0, 200, 100), "MARIO");
		GUI.Label (new Rect (rW*200, rH*0, 200, 100), "WORLD");
		GUI.Label (new Rect (rW*280, rH*0, 200, 100), "TIME");

		GUI.Label (new Rect (rW*40, rH*10, 200, 100), Mario.GetComponent<MarioControllerScript>().getScore().ToString("000000"));
		GUI.Label (new Rect (rW*140, rH*10, 200, 100), "*");
		GUI.Label (new Rect (rW*150, rH*10, 200, 100), Mario.GetComponent<MarioControllerScript>().getCoins().ToString("00"));
		if (Mario.GetComponent<MarioControllerScript> ().getLastLevel() == "Level_R_K" ||
		    Mario.GetComponent<MarioControllerScript> ().getLastLevel() == "Level_R_K_Pipe")
						levelName = "R-K";
		GUI.Label (new Rect (rW*210, rH*10, 200, 100), levelName);
		GUI.Label (new Rect (rW*290, rH*10, 200, 100), Mario.GetComponent<MarioControllerScript>().getTime().ToString("000"));
	}
}
