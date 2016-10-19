using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

public class GameNetworking : NetworkManager
{

	void Awake ()
	{
		//add a display of how many players in match
	}

	void Start(){
		NetworkManager.singleton.StartMatchMaker ();
		//_networkMatch = gameObject.AddComponent<NetworkMatch> ();

	}

	public void hostMatch ()
	{

		string matchName = GameObject.Find ("MatchNameField").transform.FindChild ("Text").GetComponent<Text> ().text;

		if (matchName.Length >= 1) {
			CreateMatchRequest newMatch = new CreateMatchRequest ();

			newMatch.name = matchName;
			newMatch.size = 4;
			newMatch.advertise = true;

			_networkMatch.CreateMatch(newMatch, OnMatchCreate);

		} else {

			Debug.Log ("<color=red>Please fill in all reqired fields!</color>");
		}
	}

	void OnMatchCreate (CreateMatchResponse in_matchResponse)
	{

		if (in_matchResponse.success) {

			Debug.Log ("<color=green>Successfully create match.</color>");

			Utility.SetAccessTokenForNetwork (in_matchResponse.networkId, new NetworkAccessToken (in_matchResponse.accessTokenString));
			NetworkServer.Listen (new MatchInfo (in_matchResponse), 1991);
		} else {

			Debug.Log ("<color=red>Unable to create match! Try again.</color>");
		}
	}



	NetworkMatch _networkMatch;

	Dropdown matchesDropDown;
	List<MatchDesc> matchList = new List<MatchDesc> ();
	public List<string> matchListStrings = new List<string> ();
	List<int> matchHostIDs = new List<int> ();
}
