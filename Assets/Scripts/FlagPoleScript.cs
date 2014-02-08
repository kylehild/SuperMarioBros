using UnityEngine;
using System.Collections;

public class FlagPoleScript : MonoBehaviour {

	public AudioClip	flagWin;
	public AudioClip	winLevel;
	public GameObject	Mario;
	public Collider2D	flagPoleCollider;
	public Collider2D	baseCollider;
	public float		moveSpeed = 3f;
	private bool		playedFlag = false;
	private bool		playedWin = false;

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.contacts[0].otherCollider == flagPoleCollider){
			GameObject.Find(" Main Camera").GetComponent<AudioSource>().Stop ();
			if(!playedFlag){
				audio.PlayOneShot (flagWin);
				playedFlag = true;
			}
			collision.gameObject.GetComponent<MarioControllerScript> ().anim.SetBool ("Win", true);
			Invoke ("MoveMario", 1.5f);
		}
	}

	void MoveMario(){
		if(!playedWin){
			audio.PlayOneShot (winLevel);
			playedWin = true;
		}

		Destroy (flagPoleCollider);
		Destroy (baseCollider);

		Mario.GetComponent<MarioControllerScript> ().anim.SetFloat ("Speed", moveSpeed);
		Mario.rigidbody2D.velocity = new Vector2 (moveSpeed, 0f);
	}
}
