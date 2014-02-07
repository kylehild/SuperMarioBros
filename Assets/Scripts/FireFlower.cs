using UnityEngine;
using System.Collections;

public class FireFlower : MonoBehaviour {

	public float 		vSpeed = 1f;
	public bool			spawned = false;
	public Vector3		originalPos;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!spawned){
			if(transform.position.y > originalPos.y+0.5f){
				spawned = true;
				rigidbody2D.velocity = new Vector2(0f,0f);
			}
			else{
				rigidbody2D.velocity = new Vector2(0, vSpeed);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		
		if(collider.gameObject.name == "Mario" && spawned){
			if(gameObject.name == "FireFlower(Clone)")
				collider.gameObject.GetComponent<MarioControllerScript>().changeState(2);
			
			Destroy(this.gameObject);
		}
	}
}
