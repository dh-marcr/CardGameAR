using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

	void Start () {
	
		changeUI (0);
	}
	
	public void changeUI(int in_UINum){

		mainUI.SetActive (false);
		hostGameUI.SetActive (false);
		joinGameUI.SetActive (false);
		gameObject.SetActive (true);

		switch (in_UINum) {

		case 0:
			mainUI.SetActive (true);
			break;

		case 1:
			hostGameUI.SetActive (true);
			break;

		case 2:
			joinGameUI.SetActive (true);
			break;

		case 3:
			gameObject.SetActive (false);
			break;
		}
	}

	public GameObject mainUI;
	public GameObject hostGameUI;
	public GameObject joinGameUI;
}
