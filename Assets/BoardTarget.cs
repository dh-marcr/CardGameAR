using UnityEngine;
using System.Collections;

public class BoardTarget : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

	public BoardCreationType creationType;
	public bool _foundAndLockedIn;
}

	public enum BoardCreationType{

		worldCenter,
		corner
	}
