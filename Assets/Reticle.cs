using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Reticle : NetworkBehaviour
{

	void Start(){

		initialization = GameObject.Find ("WorldCenter").GetComponentInChildren<GameInitialization> ();
	}

	void OnTriggerEnter (Collider in_other)
	{
		if (in_other.tag == "StarePoint") {
			netID = in_other.GetComponent<NetworkIdentity> ();
			if (netID.hasAuthority) {
				initialization.CmdUnassignAuthority (netID);
			}
			initialization.CmdAssignAuthority (netID);
			in_other.GetComponent<StarePointController> ().staring = true;
		}
	}

	void OnTriggerExit (Collider in_other)
	{
		if (in_other.tag == "StarePoint") {
			in_other.GetComponent<StarePointController>().staring = false;
			if (netID) {
				initialization.CmdUnassignAuthority (netID);
				netID = null;
			}
		}
	}

	GameInitialization initialization;
	public NetworkIdentity netID;
}
