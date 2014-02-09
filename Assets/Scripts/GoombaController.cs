using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour {

	public float 		speed = -2.5f;
	public float 		flipping = 0f;
	public float 		squishTimer = 10f;
	public bool 		facingRight = true;
	public bool 		squished = false;
	public bool			started = false;
	private	GameObject	rightBoundary;
	public Animator		anim;
	public Collider2D	headCollider;
	public Collider2D	bodyCollider;
	public Collider2D	footCollider;
	public Collider2D	rightFootCollider;
	public Collider2D	leftFootCollider;
	public Rigidbody2D	rigidBody;
	public AudioClip	stomp;
	public AudioClip	bumped;

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
			audio.PlayOneShot(stomp);
			gameObject.layer = LayerMask.NameToLayer("Default");
			Destroy(headCollider);
			Destroy(bodyCollider);
			Destroy(rigidBody);
			Destroy(footCollider);
			Destroy(rightFootCollider);
			Destroy(leftFootCollider);
			collision.gameObject.GetComponent<MarioControllerScript>().addScore(100);
			anim.SetBool("Squished", true);
		}
		else if(collision.contacts[0].otherCollider == bodyCollider){
			if(collision.gameObject.tag == "Block"){
				KillGoomba ();
			}
			else if(collision.gameObject.name != "Mario" && flipping == 0f
			   && collision.gameObject.layer != LayerMask.NameToLayer("Camera")) 
				Flip ();
			else if(collision.gameObject.name == "Mario" &&
			        collision.gameObject.GetComponent<MarioControllerScript>().getState() > 2)
				KillGoomba();
		}
	}

	void KillGoomba(){
		audio.PlayOneShot(bumped);
		rigidbody2D.AddForce (new Vector2 (0f, 1000f));

		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;

		Destroy(headCollider);
		Destroy(bodyCollider);
		Destroy(footCollider);
		Destroy(rightFootCollider);
		Destroy(leftFootCollider);

		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";

		Invoke ("DestroyGoomba", 3);
	}
	
	public void DestroyGoomba(){
		Destroy(gameObject);
	}
}
