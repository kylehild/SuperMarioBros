using UnityEngine;
using System.Collections;

public class FlagPoleScript : MonoBehaviour {

	public AudioClip	flagWin;
	public AudioClip	winLevel;
	private bool		playedFlag = false;
	private bool		playedWin = false;

	void OnTriggerEnter2D(Collider2D collider){
		GameObject.Find(" Main Camera").GetComponent<AudioSource>().Stop ();
		if(!playedFlag){
			audio.PlayOneShot (flagWin);
			playedFlag = true;
		}
		collider.gameObject.GetComponent<MarioControllerScript> ().anim.SetBool ("Win", true);
		Invoke ("MoveMario", 1.5f);
	}

	void MoveMario(){
		if(!playedWin){
			audio.PlayOneShot (winLevel);
			playedWin = true;
		}
		Debug.Log ("Move Mario to end");
	}
}
