
using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	public GameObject 		Mario;
	public float			u;
	public Vector3			offset = new Vector3(0f, 4.8f, -5);
	public Vector3			start = new Vector3(3.8f, 4.8f, -5);
	public AudioClip		gameMusic;

	// Use this for initialization
	void Start() {
		transform.position = Mario.transform.position + start;
		audio.PlayOneShot (gameMusic);
	}

	// Update is called once per frame
	void Update () {
		if(Mario != null){
			Vector3 poiV3 = Mario.transform.position + offset;
			Vector3 currPos = transform.position;
			if(currPos.x < poiV3.x){
				Vector3 pos = (1 - u) * currPos + u * poiV3;
				pos.y = currPos.y;
				transform.position = pos;
			}
		}
	}
}
