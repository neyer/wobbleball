using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

	// Use this for initialization
	private Vector3 initialPosition;
	public float maximumMoveDistance;
	private float maximumMoveDistanceSq;
	void Start () {
		initialPosition = transform.position;	
		maximumMoveDistanceSq = maximumMoveDistance * maximumMoveDistance;
	}
	
	// Update is called once per frame
	void Update () {
		//check to see if you've moved too far from your original position
		Vector3 delta = (transform.position - this.initialPosition);
		if (delta.sqrMagnitude > maximumMoveDistanceSq) {
			print (string.Format("You lose the game: {0}!", delta.sqrMagnitude));
		}
	
	}
}
