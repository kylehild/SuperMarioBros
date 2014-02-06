using UnityEngine;
using System.Collections;

public class CheckPointScript : MonoBehaviour {

	public GameObject	camera;
	public GameObject	Mario;

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.name == "Mario"){
			Vector3 newLoc = new Vector3(82.8f, 4.8f, -5);
			camera.GetComponent<CameraFollower>().setStart(newLoc);
			//move Mario too
		}
	}
}
