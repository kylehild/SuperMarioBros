using UnityEngine;
using System.Collections;

public class PipeScript : MonoBehaviour {
	
	public Collider2D	boxCollider;
	public Collider2D	frontCollider;
	public Collider2D	endCollider;
	public KeyCode		key1 = KeyCode.RightArrow;
	public KeyCode		key2 = KeyCode.D;
	public string		levelName = "Level_1_1";
	public Vector3		startPos = new Vector3(3f, 1.5f, 0f);

	void OnCollisionEnter2D(Collision2D collision){

		if(collision.contacts[0].otherCollider == endCollider){
			collision.gameObject.GetComponent<MarioControllerScript>().inPipe = false;
			collision.gameObject.GetComponent<MarioControllerScript>().goingDown = false;

			collision.gameObject.GetComponent<MarioControllerScript>().setMarioStart(startPos);
			Application.LoadLevel(levelName);
		}

	}

	void OnTriggerStay2D(Collider2D collider){
		if((Input.GetKey(key1) || Input.GetKey(key2))){
			collider.gameObject.GetComponent<MarioControllerScript>().inPipe = true;
			DestroyObject(boxCollider);
			DestroyObject(frontCollider);
		}
	}
	
}
