using UnityEngine;
using System.Collections;

public class CheckPointScript : MonoBehaviour {

	public GameObject		Mario;
	public BoxCollider2D	pipeCollider;
	public BoxCollider2D	pipeTrigger;

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.name == "Mario"){
			pipeCollider.center = new Vector2(0f, 1f);
			pipeTrigger.center = new Vector2(0f, -1f);
			Vector3 newMario = new Vector3(83f, 1.5f, 0);
			Mario.GetComponent<MarioControllerScript>().setMarioStart(newMario);
		}
	}
}
