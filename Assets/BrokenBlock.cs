using UnityEngine;
using System.Collections;

public class BrokenBlock : MonoBehaviour {

	public float	hForce = 400f;
	public float	vForce = 400f;
	public int		loc;

	// Use this for initialization
	void Start () {

		if(loc == 0)
			rigidbody2D.AddForce(new Vector2(hForce, vForce));
		else if(loc == 1)
			rigidbody2D.AddForce(new Vector2(-hForce, vForce));
		else if(loc == 2)
			rigidbody2D.AddForce(new Vector2(hForce, 0f));
		else
			rigidbody2D.AddForce(new Vector2(-hForce, 0f));
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.position.y < -2f)
			Destroy(this.gameObject);
	
	}

	public void SetLoc(int newLoc){
		loc = newLoc;
	}
}
