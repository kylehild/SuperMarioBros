using UnityEngine;
using System.Collections;

public class ItemBlocks : MonoBehaviour {
	
	public bool			hit = false;
	public bool			finishedHit = false;
	public float		blockAnimation = 2f;
	private Vector3		originalPos;
	private Animator	anim;
	private GameObject	boundary;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		boundary = GameObject.Find("LeftBoundary");
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(hit && !finishedHit){
			Vector3 pos = transform.position;
			if(blockAnimation != 0f){
				pos.y += 0.1f;
				blockAnimation--;
			}
			else{
				pos.y -= 0.1f;
				if(pos == originalPos) finishedHit = true;
			}
			transform.position = pos;
		}
		
		if(boundary.transform.position.x-1.0f > transform.position.x+0.5f)
			Destroy(this);
	}
	
	void OnCollisionEnter2D(Collision2D collision){
		
		if(collision.gameObject.name == "Mario"){
			Vector3 marioPos = collision.gameObject.transform.position;
			marioPos.y += 0.5f;
			Vector3 translatedPos = marioPos - transform.position;
			//Debug.Log(translatedPos);
			
			if(translatedPos.y > 0.99f){ //hit on top
				//Debug.Log("Hit on top");
			}
			else if(translatedPos.y < -0.99f){//hit below
				hit = true;
				anim.SetTrigger("Hit");
			}
			else{ //hit on the side
				//Debug.Log("Hit on side");
			}
		}
	}
}