using UnityEngine;
using System.Collections;

public class MushroomController : MonoBehaviour {
	
	public Collider2D	marioHeadCollider = null;
	public Collider2D	bodyCollider;
	public Collider2D	baseCollider;
	public Collider2D	rightFootCollider;
	public Collider2D	leftFootCollider;
	public float 	vSpeed = 1f;
	public float 	hSpeed = 8f;
	public float	flipping = 0f;
	public bool		spawned = false;
	public Vector3	originalPos;
	public bool		grounded = false;
	private Vector2	position;
	
	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(spawned){
			Vector3 vel = rigidbody2D.velocity;
			vel.x = hSpeed;
			rigidbody2D.velocity = vel;
		}
		else{
			if(transform.position.y > originalPos.y+0.5f){
				spawned = true;
				rightFootCollider.enabled = true;
				leftFootCollider.enabled = true;
				baseCollider.enabled = true;
				bodyCollider.enabled = true;
				rigidbody2D.gravityScale = 5;
			}
			else{
				rigidbody2D.velocity = new Vector2(0, vSpeed);
			}
		}
		
		
		if(flipping != 0) flipping--;
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		if(collision.contacts[0].otherCollider == bodyCollider && flipping == 0f){
			if(collision.gameObject.layer != LayerMask.NameToLayer("Camera")){
				flipping = 2f;
				hSpeed *= -1;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		
		if(collider.gameObject.name == "Mario" && spawned){
			if(gameObject.name == "Mushroom(Clone)"){
				if(collider.gameObject.GetComponent<MarioControllerScript>().getState() == 0)
					collider.gameObject.GetComponent<MarioControllerScript>().changeState(1);
				else if(collider.gameObject.GetComponent<MarioControllerScript>().getState() == 3)
					collider.gameObject.GetComponent<MarioControllerScript>().changeState(4);
			}
			else if(gameObject.name == "FlipMushroom(Clone)"){
				GameObject.Find (" Main Camera").GetComponent<CameraFollower>().flipped = true;
			}
			else
				collider.gameObject.GetComponent<MarioControllerScript>().addLife();
			
			Destroy(this.gameObject);
		}
	}
}
