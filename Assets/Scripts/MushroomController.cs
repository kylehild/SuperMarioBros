using UnityEngine;
using System.Collections;

public class MushroomController : MonoBehaviour {

	public Collider2D	marioHeadCollider = null;
	public Collider2D	rightCollider;
	public Collider2D	leftCollider;
	public Collider2D	baseCollider;
	public float 		vSpeed = 3f;
	public float 		hSpeed = 5f;
	public bool			spawned = false;
	public Vector3		originalPos;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(spawned){
			Vector2 vel = new Vector2(hSpeed, 0);
			rigidbody2D.velocity = vel;
		}
		else{
			if(transform.position.y > originalPos.y+1){
				spawned = true;
				rightCollider.enabled = true;
				leftCollider.enabled = true;
				baseCollider.enabled = true;
				rigidbody2D.gravityScale = 50;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){

		if(collision.contacts[0].otherCollider == rightCollider || 
		   collision.contacts[0].otherCollider == leftCollider){
			hSpeed *= -1;
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		if(collider.gameObject.name == "Mario"){
			if(marioHeadCollider == null){
				marioHeadCollider = collider.gameObject.GetComponent<MarioControllerScript>().headCollider;
			}
			if(spawned){
				Debug.Log("Grow mario");
				Destroy(this.gameObject);
			}
			else if(collider == marioHeadCollider){
				Debug.Log("Spawning");
				Vector2 vel = new Vector2(0, vSpeed);
				rigidbody2D.velocity = vel;
			}
		}
	}
}
