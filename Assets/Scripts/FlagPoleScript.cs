using UnityEngine;
using System.Collections;

public class FlagPoleScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
		collider.gameObject.GetComponent<MarioControllerScript> ().anim.SetBool ("Win", true);
	}
}
