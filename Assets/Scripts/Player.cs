using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

using System.Collections;

public class Player : NetworkBehaviour {

	public float speed;

	private Rigidbody rb;
	private int cubesEatenCount = 0;
	private Vector3 baseScale;
	private NetworkIdentity networkId;
	public bool isAlive = true;

	private Transform startTransform = null;


	protected bool _canControl = true;
	protected Collider _collider;
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		networkId = GetComponent<NetworkIdentity> ();
		baseScale = this.transform.localScale;
		_collider = GetComponent<Collider>();

		startTransform = this.transform;

		NetworkGameManager.sInstance.RegisterPlayer (this);
	}

	void updateLocalPlayer()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

	}
	void FixedUpdate ()
	{		
		updateLocalPlayer ();			
	}

	[Server]
	public void Respawn()
	{
		EnablePlayerBall(true);
		RpcRespawn();
	}
	[ClientRpc]
	void RpcRespawn()
	{
		EnablePlayerBall(true);
		this.transform.position = startTransform.position;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;


	}
	//We can't disable the whole object, as it would impair synchronisation/communication
	//So disabling mean disabling collider & renderer only
	public void EnablePlayerBall(bool enable)
	{
		GetComponent<Renderer>().enabled = enable;
		isAlive = enable;
		_collider.enabled = isServer && enable;
		_canControl = enable;
	}

	private void updateScale() {
		this.transform.localScale = this.baseScale * this.cubesEatenCount;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("PickUp")) {
			other.gameObject.SetActive (false);
			++this.cubesEatenCount;
		} else if (other.gameObject.CompareTag ("Finish")) 
		{
			print ("Now you are off the edge!");
			EnablePlayerBall (false);

		}
	}
}
