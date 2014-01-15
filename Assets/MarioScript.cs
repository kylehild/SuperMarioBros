using UnityEngine;
using System.Collections;

public class MarioScript : MonoBehaviour {

	public GameObject 	marioPreFab;
	public float 		speed = 0.001f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float h = Input.GetAxis("Horizontal");
		Vector2 pos = transform.position;
		pos.x += h*speed;
		
		if (Input.GetKey (KeyCode.X)) {
			pos.y += speed;
		}
		transform.position = pos;
	}
}
