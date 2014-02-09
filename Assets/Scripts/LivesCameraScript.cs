using UnityEngine;
using System.Collections;

public class LivesCameraScript : MonoBehaviour {

	public GameObject	Mario;

	// Use this for initialization
	void Start () {
		Vector3 disp = new Vector3 (1.25f, 0.5f, 0f);
		transform.position = Mario.transform.position + disp;
	}
}
