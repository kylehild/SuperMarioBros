using UnityEngine;
using System.Collections;

public class MenuBarScript : MonoBehaviour {

	public GUISkin		fontSkin;
	public GameObject	Mario;
	public string		levelName = "1-1";

	void OnGUI () {
		GUI.skin = fontSkin;
		GUI.Label (new Rect (40, 0, 200, 100), "MARIO");
		GUI.Label (new Rect (200, 0, 200, 100), "WORLD");
		GUI.Label (new Rect (280, 0, 200, 100), "TIME");

		GUI.Label (new Rect (40, 10, 200, 100), Mario.GetComponent<MarioControllerScript>().getScore().ToString("000000"));
		GUI.Label (new Rect (140, 10, 200, 100), "*");
		GUI.Label (new Rect (150, 10, 200, 100), Mario.GetComponent<MarioControllerScript>().getCoins().ToString("00"));
		GUI.Label (new Rect (210, 10, 200, 100), levelName);
		GUI.Label (new Rect (290, 10, 200, 100), Mario.GetComponent<MarioControllerScript>().getTime().ToString("000"));
	}
}
