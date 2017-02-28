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

			GameInitialization[] inits = FindObjectsOfType<GameInitialization> ();

			foreach (GameInitialization i in inits) {

				if (i.transform != transform) {
					Destroy (i.gameObject);
				}
			}
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

		setupPlayerPoints();
	}

	void setupPlayerPoints()
	{

		LobbyManager lm = FindObjectOfType<LobbyManager>();
		playerCount = lm._playerNumber;

		GameObject starePoint = (GameObject)Resources.Load("StarePoint");
		//lm.spawnPrefabs.Add (starePoint);

		for (int i = 0; i < playerCount; i++)
		{

			GameObject ob = (GameObject)Instantiate(Resources.Load("StarePoint"), Vector3.zero, Quaternion.identity) as GameObject;

			if (ob != null) {
				destroyableObjects.Add (ob);
				CmdSpawnPoint (ob);
			} else {
				Debug.Log ("<color=cayan>No object to spawn on server</color>");
			}
		}

		assignPosition();
	}

	[Command]
	public void CmdSpawnPoint(GameObject in_point)
	{
		Debug.Log("<color=red>object to spawn : " + in_point + "</color>");
		if (in_point != null) {
			NetworkServer.Spawn (in_point);
		} else {
			Debug.Log ("<color=blue>Cannot spawn on server</color>");
		}
	}

	void assignPosition(){

		StarePointController[] playerPoints = FindObjectsOfType<StarePointController>();

		foreach (StarePointController stare in playerPoints)
		{
			stare.transform.parent = transform;
			stare.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
		}

		float newXPosition = findNewPosition (boardScale.x);
		float newZPosition = findNewPosition (boardScale.z);

		switch (playerCount) {

		case 2:

			if (newXPosition > newZPosition) {
				playerPoints [0].transform.position = new Vector3 (newXPosition, playerPoints [0].transform.position.y, playerPoints [0].transform.position.z);
				playerPoints [1].transform.position = new Vector3 (-newXPosition, playerPoints [1].transform.position.y, playerPoints [1].transform.position.z);
			} else {
				playerPoints [0].transform.position = new Vector3 (newZPosition, playerPoints [0].transform.position.y, playerPoints [0].transform.position.z);
				playerPoints [1].transform.position = new Vector3 (-newZPosition, playerPoints [1].transform.position.y, playerPoints [1].transform.position.z);
			}
			break;

		case 3:
			if (newXPosition > newZPosition) {
				playerPoints [0].transform.position = new Vector3 (playerPoints [0].transform.position.x, playerPoints [0].transform.position.y, newZPosition);
				playerPoints [1].transform.position = new Vector3 (newXPosition, playerPoints [1].transform.position.y, -newZPosition * 0.5f);
				playerPoints [2].transform.position = new Vector3 (-newXPosition, playerPoints [2].transform.position.y, -newZPosition * 0.5f);
			} else {
				playerPoints [0].transform.position = new Vector3 (newXPosition, playerPoints [0].transform.position.y, playerPoints [0].transform.position.z);
				playerPoints [1].transform.position = new Vector3 (-newXPosition * 0.5f, playerPoints [1].transform.position.y, newZPosition);
				playerPoints [2].transform.position = new Vector3 (-newXPosition * 0.5f, playerPoints [2].transform.position.y, -newZPosition);
			}
			break;

		case 4:
			playerPoints [0].transform.position = new Vector3 (newXPosition, playerPoints [0].transform.position.y, playerPoints [0].transform.position.z);
			playerPoints [1].transform.position = new Vector3 (-newXPosition, playerPoints [1].transform.position.y, playerPoints [1].transform.position.z);
			playerPoints [2].transform.position = new Vector3 (playerPoints [2].transform.position.x, playerPoints [2].transform.position.y, newZPosition);
			playerPoints [3].transform.position = new Vector3 (playerPoints [3].transform.position.x, playerPoints [3].transform.position.y, -newZPosition);
			break;
		}
		clearDestroyables();
	}

	void clearDestroyables()
	{
		/*
		if (isClient) {

			StarePointController[] starePoints = FindObjectsOfType<StarePointController> ();
			List<GameObject> tempObjects = new List<GameObject> ();

			for (int i = 0; i < starePoints.Length; i++) {
				if (!destroyableObjects.Contains(starePoints[i].gameObject)) {
					tempObjects.Add (starePoints [i].gameObject);
				}
			}

			destroyableObjects.Clear ();
			destroyableObjects = tempObjects;
			starePoints = null;
		}*/

		foreach (GameObject go in destroyableObjects)
		{
			//Destroy(go);
		}

		//destroyableObjects.Clear();
	}

	float findNewPosition (float in_size)
	{

		float whole = in_size * 10;
		float half = whole * 0.5f;
		float actual = half * 0.65f;

		return actual;
	}

	[Command]
	public void CmdAssignAuthority(NetworkIdentity in_ID){

		in_ID.AssignClientAuthority (connectionToClient);
	}

	[Command]
	public void CmdUnassignAuthority (NetworkIdentity in_ID){

		in_ID.RemoveClientAuthority (connectionToClient);
	}

	public List<GameObject> destroyableObjects = new List<GameObject>();

	public int playerCount;

	Transform target;

	[SyncVar]
	public Vector3 boardScale;
}
