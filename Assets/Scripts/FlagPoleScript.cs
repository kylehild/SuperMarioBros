using UnityEngine;
using System.Collections;

public class FlagPoleScript : MonoBehaviour {
	
	public AudioClip	flagWin;
	public AudioClip	winLevel;
	public GameObject	Mario;
	public Collider2D	collider400;
	public Collider2D	collider800;
	public Collider2D	collider2000;
	public Collider2D	collider5000;
	public Collider2D	baseCollider;
	public float		moveSpeed = 3f;
	private bool		playedFlag = false;
	private bool		playedWin = false;
	
	void OnCollisionEnter2D(Collision2D collision){
		if(collision.contacts[0].otherCollider == collider400)
			Mario.GetComponent<MarioControllerScript>().addScore(400);
		else if(collision.contacts[0].otherCollider == collider800)
			Mario.GetComponent<MarioControllerScript>().addScore(800);
		else if(collision.contacts[0].otherCollider == collider2000)
			Mario.GetComponent<MarioControllerScript>().addScore(2000);
		else if(collision.contacts[0].otherCollider == collider5000)
			Mario.GetComponent<MarioControllerScript>().addScore(5000);
		
		if(collision.contacts[0].otherCollider == collider400 ||
		   collision.contacts[0].otherCollider == collider800 ||
		   collision.contacts[0].otherCollider == collider2000 ||
		   collision.contacts[0].otherCollider == collider5000){
			
			Destroy (collider400);
			Destroy (collider800);
			Destroy (collider2000);
			Destroy (collider5000);
			
			GameObject.Find(" Main Camera").GetComponent<AudioSource>().Stop ();
			if(!playedFlag){
				audio.PlayOneShot (flagWin);
				playedFlag = true;
			}
			
			collision.gameObject.GetComponent<MarioControllerScript> ().anim.SetFloat ("Speed", 0f);
			collision.gameObject.GetComponent<MarioControllerScript> ().anim.SetBool ("Win", true);
			Invoke ("MoveMario", 1.5f);
		}
	}
	
	void MoveMario(){
		if(!playedWin){
			Mario.GetComponent<MarioControllerScript> ().anim.SetBool ("Win", false);
			Mario.GetComponent<MarioControllerScript> ().anim.SetFloat ("Speed", moveSpeed);
			audio.PlayOneShot (winLevel);
			playedWin = true;
		}
		
		Destroy (baseCollider);
		
		Mario.rigidbody2D.velocity = new Vector2 (moveSpeed, 0f);
	}
}

