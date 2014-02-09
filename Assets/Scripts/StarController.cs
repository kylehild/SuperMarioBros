using UnityEngine;
using System.Collections;

public class StarController : MonoBehaviour {

	public Collider2D	footCollider;
	public Collider2D	rightCollider;
	public Collider2D	leftCollider;
	public float 		hSpeed = 4f;
	public float 		vSpeed = 1f;
	public float 		hForce = 100f;
	public float 		vForce = 100f;
	public float		flipping = 0f;
	public bool			spawned = false;
	public Vector3		originalPos;
	public bool			grounded = false;
	private Vector2		position;
	
	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(spawned){
			rigidbody2D.velocity = new Vector2(-hSpeed, rigidbody2D.velocity.y);
		}
		else{
			if(transform.position.y > originalPos.y+0.5f){
				spawned = true;
				rigidbody2D.AddForce(new Vector2(300f, 300f));
				footCollider.enabled = true;
				leftCollider.enabled = true;
				rightCollider.enabled = true;
				rigidbody2D.gravityScale = 2f;
			}
			else{
				rigidbody2D.velocity = new Vector2(0, vSpeed);
			}
		}
		
		
		if(flipping != 0) flipping--;
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		if((collision.contacts[0].otherCollider == leftCollider ||
		   collision.contacts[0].otherCollider == rightCollider) && flipping == 0f){
			if(collision.gameObject.layer != LayerMask.NameToLayer("Camera")){
				flipping = 2f;
				hSpeed *= -1;
			}
		}

		if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && spawned){
			rigidbody2D.AddForce(new Vector2(hForce, vForce));
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.name == "Mario" && spawned){
			if(collider.gameObject.GetComponent<MarioControllerScript>().getState() == 0)
				collider.gameObject.GetComponent<MarioControllerScript>().changeState(3);
			else if(collider.gameObject.GetComponent<MarioControllerScript>().getState() == 1)
				collider.gameObject.GetComponent<MarioControllerScript>().changeState(4);
			else
				collider.gameObject.GetComponent<MarioControllerScript>().changeState(5);
			Destroy(this.gameObject);
		}
	}
}
