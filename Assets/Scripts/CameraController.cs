using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization

	public GameObject followObject;
	public Vector3 offset;
	void Start () {
		offset = transform.position - followObject.transform.position;
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
		transform.position = followObject.transform.position + offset;
	}
}
