using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour {

	public float 		speed = -2.5f;
	public float 		flipping = 0f;
	public float 		squishTimer = 12f;
	public bool 		facingRight = true;
	public bool 		squished = false;
	public bool			started = false;
	public bool 		killedMario = false;
	private	GameObject	rightBoundary;
	private Animator	anim;
	public Collider2D	headCollider;
	public Collider2D	bodyCollider;
	public Collider2D	footCollider;
	public Rigidbody2D	rigidBody;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rightBoundary = GameObject.Find ("RightBoundary");
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!squished && rightBoundary.transform.position.x >= transform.position.x
		   && rigidbody2D.velocity.x == 0){

			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed;
			rigidbody2D.velocity = vel;
			started = true;
		}

		if(started && !squished){
			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed;
			rigidbody2D.velocity = vel;
		}

		if(flipping != 0) flipping--;

		if(squished)
			Invoke ("DestroyGoomba", squishTimer);
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		speed *= -1;
		flipping = 2f;
	}

	void OnCollisionEnter2D(Collision2D collision){
		if(collision.contacts[0].otherCollider == headCollider
		   && collision.gameObject.name == "Mario"){
			squished = true;
			gameObject.layer = LayerMask.NameToLayer("Default");
			DestroyObject(headCollider);
			DestroyObject(bodyCollider);
			DestroyObject(rigidBody);
			DestroyObject(footCollider);
			anim.SetTrigger("Squished");
		}
		else if(collision.contacts[0].otherCollider == bodyCollider){
			if(collision.gameObject.name != "Mario" && flipping == 0f
			   && collision.gameObject.layer != LayerMask.NameToLayer("Camera")) 
				Flip ();
		}
	}

	public void DestroyGoomba(){
		Destroy(gameObject);
	}
}
