using UnityEngine;
using System.Collections;

public class OutPipeScript : MonoBehaviour {

	public BoxCollider2D	boxCollider;
	public AudioClip		outPipeSound;
	private bool			hasPlayed = false;

	void OnTriggerExit2D(Collider2D collider){
		if(collider.gameObject.name == "Mario" && 
		   collider.gameObject.GetComponent<MarioControllerScript>().footCollider == collider){
			if(!hasPlayed){
				audio.PlayOneShot(outPipeSound);
				hasPlayed = true;
			}
			boxCollider.center = new Vector2 (0f, 1f);
			collider.gameObject.GetComponent<MarioControllerScript> ().setUp (false);
			Vector3 newMario = new Vector3(83f, 1.5f, 0);
			collider.gameObject.GetComponent<MarioControllerScript>().setMarioStart(newMario);
		}
	}
}
