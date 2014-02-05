using UnityEngine;
using System.Collections;

public class StationaryCoinScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
		collider.gameObject.GetComponent<MarioControllerScript> ().numCoins++;
		DestroyObject (gameObject);
	}
}
