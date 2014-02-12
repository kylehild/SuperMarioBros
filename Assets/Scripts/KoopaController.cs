using UnityEngine;
using System.Collections;

public class KoopaController : MonoBehaviour {
	
	public float 		speed = -2.5f;
	public float 		flipping = 0f;
	public float 		squishTime = 0f;
	public float 		spawnTime = 0f;
	public bool 		facingRight = true;
	public bool 		squished = false;
	public bool			started = false;
	public bool			waiting = false;
	public bool			spawning = false;
	public bool			hit = false;
	public bool 		canHit = false;
	public Animator		anim;
	public GameObject	rightBoundary;

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
		rightBoundary = GameObject.Find ("RightBoundary");
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(hit){
			squished = false;
			anim.SetBool("Squished", false);
			waiting = false;
			spawning = false;
			anim.SetBool("Respawn", false);
			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed*4f;
			rigidbody2D.velocity = vel;
			anim.SetFloat("Speed", 0f);
		}

		if(waiting && squishTime > 0){
			squishTime--;
		}
		else if(waiting){
			spawning = true;
			spawnTime = 100f;
			anim.SetBool("Respawn", true);
			waiting = false;
		}

		if(spawning && spawnTime > 0)
			spawnTime--;
		else if(spawning){
			anim.SetFloat("Speed", Mathf.Abs(speed));
			anim.SetBool("Respawn", false);
			canHit = false;
			spawning = false;
		}

		if(squished && anim.GetFloat ("Speed") > 0){
			anim.SetBool ("Squished", false);
			squished = false;
		}

		if(!hit && !squished && rightBoundary.transform.position.x >= transform.position.x && 
		   rigidbody2D.velocity.x == 0){
			
			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed;
			rigidbody2D.velocity = vel;
			started = true;
		}
		
		if(!hit && started && !squished){
			Vector3 vel = rigidbody2D.velocity;
			vel.x = speed;
			rigidbody2D.velocity = vel;
		}

		if(flipping != 0) flipping--;
		
		if(squished && !waiting){
			squishTime = 300f;
			waiting = true;
		}
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
		if(collision.contacts[0].otherCollider == headCollider &&
		   collision.gameObject.name == "Mario" &&
		   collision.contacts[0].collider == collision.gameObject.GetComponent<MarioControllerScript>().footCollider){
			if(!squished){
				squished = true;
				audio.PlayOneShot(stomp);
				anim.SetBool("Squished", true);
				rigidbody2D.velocity = new Vector2(0f, 0f);
				canHit = true;
			}
			else if(canHit){
				audio.PlayOneShot(bumped);
				if(transform.position.x-collision.gameObject.transform.position.x > 0f)
					speed *= -1;
				
				if(hit){
					anim.SetBool("Hit", false);
					hit = false;
					rigidbody2D.velocity = new Vector2(0f, 0f);
				}
				else{
					anim.SetBool("Hit", true);
					hit = true;
					rigidbody2D.velocity = new Vector2(speed*4f, 0f);
				}
			}
		}
		else if(collision.contacts[0].otherCollider == bodyCollider){
			if(collision.gameObject.tag == "Block"){
				KillKoopa ();
			}
			else if(collision.gameObject.name != "Mario" && collision.gameObject.name != "Goomba"
			        && flipping == 0f && collision.gameObject.layer != LayerMask.NameToLayer("Camera")) 
				Flip ();
			else if(collision.gameObject.name == "Mario" &&
			        collision.gameObject.GetComponent<MarioControllerScript>().Invincible())
				KillKoopa();
			else if(collision.gameObject.name == "GreenKoopa" &&
			        collision.gameObject.GetComponent<Animator>().GetBool("Hit"))
				KillKoopa();
		}
	}
	
	public void KillKoopa(){
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
		
		GameObject.Find("Mario").GetComponent<MarioControllerScript>().addScore(100);
		
		Invoke ("DestroyKoopa", 3);
	}
	
	public void DestroyKoopa(){
		Destroy(gameObject);
	}
}
