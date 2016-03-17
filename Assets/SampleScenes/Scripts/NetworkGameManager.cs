using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NetworkGameManager : NetworkBehaviour
{
    static public List<Player> players = new List<Player>();
    static public NetworkGameManager sInstance = null;

    public GameObject uiScoreZone;
    public Font uiScoreFont;
    
    [Header("Gameplay")]
    //Those are sorte dby level 0 == lowest etc...
    public GameObject spawnCubePrefab;

    [Space]

    protected bool _spawningAsteroid = true;
    protected bool _running = true;

    void Awake()
    {
        sInstance = this;
    }

    void Start()
    {
        if (isServer)
        {
            StartCoroutine(AsteroidCoroutine());
        }
			        
    }

    [ServerCallback]
    void Update()
    {
        if (!_running || players.Count == 0)
            return;

        bool allDestroyed = true;
        for (int i = 0; i < players.Count; ++i)
        {
			allDestroyed &= (!players[i].isAlive);
        }

        if(allDestroyed)
        {
			print ("all the ships is dead!");
			RestartLevel ();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

		print("starting client");
		ClientScene.RegisterPrefab(spawnCubePrefab);        
    }

	public void RegisterPlayer(Player player) {
		players.Add (player);
	}

	public void RestartLevel() {
		SceneManager.LoadScene("base");

	}
    
    IEnumerator AsteroidCoroutine()
    {
        const float MIN_TIME = 5.0f;
        const float MAX_TIME = 10.0f;

        while(_spawningAsteroid)
        {
            yield return new WaitForSeconds(Random.Range(MIN_TIME, MAX_TIME));

            Vector2 dir = Random.insideUnitCircle;
            Vector3 position = Vector3.zero;

            if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {//make it appear on the side
                position = new Vector3( Mathf.Sign(dir.x)* Camera.main.orthographicSize * Camera.main.aspect, 
                                        0, 
                                        dir.y * Camera.main.orthographicSize);
            }
            else
            {//make it appear on the top/bottom
                position = new Vector3(dir.x * Camera.main.orthographicSize * Camera.main.aspect, 
                                        0,
                                        Mathf.Sign(dir.y) * Camera.main.orthographicSize);
            }

            //offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
            position -= position.normalized * 0.1f;
            


			GameObject cube = Instantiate(spawnCubePrefab, position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f)) as GameObject;

            NetworkServer.Spawn(cube);

        }
    }


    public IEnumerator WaitForRespawn(Player ship)
    {
        yield return new WaitForSeconds(4.0f);

        ship.Respawn();
    }
}
