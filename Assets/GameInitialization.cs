using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using Prototype.NetworkLobby;

public class GameInitialization : NetworkBehaviour
{

	void Start ()
	{
		if (isLocalPlayer) {
			target = GameObject.Find ("WorldCenter").transform;
			transform.parent = target;
			spawnBoard ();

			DefaultTrackableEventHandler trackable = target.GetComponent<DefaultTrackableEventHandler> ();
			trackable.targetFoundEvent.RemoveAllListeners ();
			trackable.targetLostEvent.RemoveAllListeners ();

			trackable.targetFoundEvent.AddListener (() => targetFound());
			trackable.targetLostEvent.AddListener (() => targetLost());


		}
	}

	void targetFound(){
		StarePointController[] starePoints= FindObjectsOfType<StarePointController>();

		foreach (StarePointController spc in starePoints) {
			spc.show ();
		}
	}

	void targetLost(){
		StarePointController[] starePoints= FindObjectsOfType<StarePointController>();

		foreach (StarePointController spc in starePoints) {
			spc.resetPoints ();
		}
	}

	void spawnBoard ()
	{

		GameObject board = (GameObject)Instantiate (Resources.Load ("TileBoard"), Vector3.zero, Quaternion.identity) as GameObject;
		board.transform.parent = transform;
		board.transform.position = Vector3.zero;
		board.transform.localScale = boardScale;

		setupPlayerPoints ();
	}

	void setupPlayerPoints ()
	{

		LobbyManager lm = FindObjectOfType<LobbyManager> ();
		playerCount = lm._playerNumber;

		GameObject starePoint = (GameObject)Resources.Load ("StarePoint");
		lm.spawnPrefabs.Add (starePoint);

		for (int i = 0; i < playerCount; i++) {

			GameObject ob = (GameObject)Instantiate (Resources.Load ("StarePoint"), Vector3.zero, Quaternion.identity) as GameObject;
			ob.transform.parent = transform;
			ob.transform.position = Vector3.zero;
			ob.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);

			playerPoints.Add (ob.transform);

			NetworkServer.SpawnWithClientAuthority (ob, connectionToClient);
		}

		float newXPosition = findNewPosition (boardScale.x);
		float newZPosition = findNewPosition (boardScale.z);

		switch (playerCount) {

		case 2:

			if (newXPosition > newZPosition) {
				playerPoints [0].position = new Vector3 (newXPosition, playerPoints [0].position.y, playerPoints [0].position.z);
				playerPoints [1].position = new Vector3 (-newXPosition, playerPoints [1].position.y, playerPoints [1].position.z);
			} else {
				playerPoints [0].position = new Vector3 (newZPosition, playerPoints [0].position.y, playerPoints [0].position.z);
				playerPoints [1].position = new Vector3 (-newZPosition, playerPoints [1].position.y, playerPoints [1].position.z);
			}
			break;

		case 3:
			if (newXPosition > newZPosition) {
				playerPoints [0].position = new Vector3 (playerPoints [0].position.x, playerPoints [0].position.y, newZPosition);
				playerPoints [1].position = new Vector3 (newXPosition, playerPoints [1].position.y, -newZPosition * 0.5f);
				playerPoints [2].position = new Vector3 (-newXPosition, playerPoints [2].position.y, -newZPosition * 0.5f);
			} else {
				playerPoints [0].position = new Vector3 (newXPosition, playerPoints [0].position.y, playerPoints [0].position.z);
				playerPoints [1].position = new Vector3 (-newXPosition * 0.5f, playerPoints [1].position.y, newZPosition);
				playerPoints [2].position = new Vector3 (-newXPosition * 0.5f, playerPoints [2].position.y, -newZPosition);
			}
			break;

		case 4:
			playerPoints [0].position = new Vector3 (newXPosition, playerPoints [0].position.y, playerPoints [0].position.z);
			playerPoints [1].position = new Vector3 (-newXPosition, playerPoints [1].position.y, playerPoints [1].position.z);
			playerPoints [2].position = new Vector3 (playerPoints [2].position.x, playerPoints [2].position.y, newZPosition);
			playerPoints [3].position = new Vector3 (playerPoints [3].position.x, playerPoints [3].position.y, -newZPosition);
			break;
		}
	}

	float findNewPosition (float in_size)
	{

		float whole = in_size * 10;
		float half = whole * 0.5f;
		float actual = half * 0.65f;

		return actual;
	}

	public int playerCount;
	List<Transform> playerPoints = new List<Transform> ();

	Transform target;

	[SyncVar]
	public Vector3 boardScale;
}
