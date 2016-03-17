using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Network;
using UnityEngine.SceneManagement;

public class NetManager : MonoBehaviour {

	// Use this for initialization

	private static NetManager m_Instance = new NetManager();

	public static NetManager instance { get { return m_Instance; } }

	void Start () {
	
	}

	public void restartScene() {
		SceneManager.LoadScene ("base");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
