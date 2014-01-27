using UnityEngine;
using System.Collections;

public class FallDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collision){
		if(collision.gameObject.name == "Mario"){
			//Destroy(collision.gameObject);
			Application.LoadLevel(Application.loadedLevelName);
		}
		else Destroy(collision.gameObject);
	}
}
