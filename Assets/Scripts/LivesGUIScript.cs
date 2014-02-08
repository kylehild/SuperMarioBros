using UnityEngine;
using System.Collections;

public class LivesGUIScript : MonoBehaviour {

	public GUISkin		fontSkin;
	public GameObject	Mario;
	public string		levelName = "1-1";

	void OnGUI () {
		GUI.skin = fontSkin;
		GUI.Label (new Rect (170, 150, 200, 100), "*");
		GUI.Label (new Rect (190, 150, 200, 100), Mario.GetComponent<MarioControllerScript>().getLives().ToString("00"));

		Invoke ("PlayLevel", 1.5f);
	}

	public void PlayLevel(){
		Application.LoadLevel (Mario.GetComponent<MarioControllerScript> ().getLastLevel ());
	}
}
