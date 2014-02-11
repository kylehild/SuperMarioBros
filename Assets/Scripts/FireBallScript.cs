using UnityEngine;
using System.Collections;

public class FireBallScript : MonoBehaviour {
	
	public Collider2D	footCollider;
	public Collider2D	bodyCollider;
	public GameObject	rightBoundary;
	public GameObject	leftBoundary;
	public float 		hSpeed = 4f;
	public float 		hForce = 500f;
	public float 		vForce = 500f;
	public float		flipping = 0f;
	public bool			start = true;
	public bool			left;

	// Use this for initialization
	void Start () {
		leftBoundary = GameObject.Find("LeftBoundary");
		rightBoundary = GameObject.Find("RightBoundary");
	}

	// Update is called once per frame
	void Update () {

		if(start){
			if(left) hSpeed *= -1;
			start = false;
		}

		rigidbody2D.velocity = new Vector2(hSpeed, rigidbody2D.velocity.y);
		
		if(flipping != 0) flipping--;

		if(transform.position.x > rightBoundary.transform.position.x - 2f ||
		   transform.position.x < leftBoundary.transform.position.x)
			DestroyFireBall();
	}
	
	void OnCollisionEnter2D(Collision2D collision){

		if(collision.gameObject.layer == LayerMask.NameToLayer("Enemies")){
			DestroyFireBall();
		}

		if(collision.contacts[0].otherCollider == footCollider){
			rigidbody2D.AddForce(new Vector2(hForce, vForce));
		}
		else if(collision.contacts[0].otherCollider == bodyCollider && flipping == 0f){
			if(collision.gameObject.layer != LayerMask.NameToLayer("Camera")){
				flipping = 2f;
				hSpeed *= -1;
			}
		}
	}

	void DestroyFireBall(){
		Destroy(gameObject);
	}
}
