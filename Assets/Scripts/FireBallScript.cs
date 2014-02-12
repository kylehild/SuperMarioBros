using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour {
	
	public Collider2D	footCollider;
	public Collider2D	bodyCollider;
	public GameObject	rightBoundary;
	public GameObject	leftBoundary;
	public GameObject	mario;
	public float 		hSpeed = 4f;
	public float 		hForce = 500f;
	public float 		vForce = 500f;
	public bool			start = true;
	public bool			left;
	public bool			exploded = false;

	// Use this for initialization
	void Start () {
		leftBoundary = GameObject.Find("LeftBoundary");
		rightBoundary = GameObject.Find("RightBoundary");
		mario = GameObject.Find("Mario");
	}

	// Update is called once per frame
	void Update () {

		if(start){
			if(left) hSpeed *= -1;
			start = false;
		}

		if(!exploded) rigidbody2D.velocity = new Vector2(hSpeed, rigidbody2D.velocity.y);
		else rigidbody2D.velocity = new Vector2(0f, 0f);

		if(transform.position.x > rightBoundary.transform.position.x - 2f ||
		   transform.position.x < leftBoundary.transform.position.x){
			DestroyFireBall();
		}	
	}
	
	void OnCollisionEnter2D(Collision2D collision){

		if(collision.gameObject.layer == LayerMask.NameToLayer("Enemies")){
			gameObject.GetComponent<Animator>().SetBool("Explode", true);
			if(collision.gameObject.name == "Goomba") 
				collision.gameObject.GetComponent<GoombaController>().KillGoomba();
			else
				collision.gameObject.GetComponent<KoopaController>().KillKoopa();
			exploded = true;
			Invoke("DestroyFireBall", 0.2f);
		}

		if(collision.contacts[0].otherCollider == footCollider){
			rigidbody2D.AddForce(new Vector2(hForce, vForce));
		}
		else if(collision.contacts[0].otherCollider == bodyCollider){
			if(collision.gameObject.layer != LayerMask.NameToLayer("Camera")){
				gameObject.GetComponent<Animator>().SetBool("Explode", true);
				exploded = true;
				Invoke("DestroyFireBall", 0.2f);
			}
		}
	}

	void DestroyFireBall(){
		mario.GetComponent<MarioControllerScript>().fireballCount--;
		Destroy(gameObject);
	}
}
