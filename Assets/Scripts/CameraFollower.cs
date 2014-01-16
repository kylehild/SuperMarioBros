using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	public Transform 	poi;
	public float		u;
	public Vector3		offset = new Vector3(3.7f, 3f, -5);

	// Use this for initialization
	void Start() {
		transform.position = poi.position + offset;
	}

	// Update is called once per frame
	void Update () {
		Vector3 poiV3 = poi.position + offset;
		Vector3 currPos = transform.position;
		if(currPos.x < poiV3.x){
			Vector3 pos = (1 - u) * currPos + u * poiV3;
			pos.y = currPos.y;
			transform.position = pos;
		}
	}
}
