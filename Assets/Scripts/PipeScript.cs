using UnityEngine;
using System.Collections;

public class PipeScript : MonoBehaviour {
	
	public Collider2D	boxCollider;
	public Collider2D	frontCollider;
	public Collider2D	endCollider;

	void OnCollisionEnter2D(Collision2D collision){

		if(collision.contacts[0].otherCollider == frontCollider){
			collision.gameObject.GetComponent<MarioControllerScript>().inPipe = true;
			DestroyObject(boxCollider);
			DestroyObject(frontCollider);
		}
		else if(collision.contacts[0].otherCollider == endCollider){
			collision.gameObject.GetComponent<MarioControllerScript>().inPipe = false;
			Application.LoadLevel("Level_1_1");
		}

	}

}
