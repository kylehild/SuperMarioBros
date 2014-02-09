using UnityEngine;
using System.Collections;

public class LivesGUIScript : MonoBehaviour {

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
		GUI.Label (new Rect (rW*170, rH*150, 200, 100), "*");
		GUI.Label (new Rect (rW*190, rH*150, 200, 100), Mario.GetComponent<MarioControllerScript>().getLives().ToString("00"));

		Invoke ("PlayLevel", 1.5f);
	}

	public void PlayLevel(){
		Application.LoadLevel (Mario.GetComponent<MarioControllerScript> ().getLastLevel ());
	}
}
