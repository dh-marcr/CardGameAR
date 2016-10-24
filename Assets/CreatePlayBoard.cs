using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class CreatePlayBoard : MonoBehaviour
{

	void Start ()
	{
		changeUIStyle (0);
	}

	public void targetFound (ImageTargetBehaviour in_target)
	{

		DebugUtils.AddToLog ("<color=green>image target </color>" + in_target.name + "<color=green>found</color>");
		_foundTarget = true;
		_boardTarget = in_target.transform.FindChild ("Origin").GetComponent<BoardTarget> ();

		if (!_buildingBoard) {
			changeUIStyle (1);
		} else {
			changeUIStyle (2);
		}
	}

	public void targetLost (ImageTargetBehaviour in_target)
	{
		
		DebugUtils.AddToLog ("<color=red>image target </color>" + in_target.name + "<color=red>lost</color>");
		changeUIStyle (0);
		_foundTarget = false;
	}

	void changeUIStyle(int in_style){

		scanner.SetActive (false);
		scaler.SetActive (false);
		scannerMessage.gameObject.SetActive (false);
		lockInButton.SetActive (false);
		startBoardSetup.SetActive (false);

		scannerBG.color = new Color32 (126, 126, 126, 100);
		scannerMessage.text = "Look for target";

		switch (in_style) {

		case 0:
			//default scanner ui
			scanner.SetActive (true);
			scannerMessage.gameObject.SetActive (true);
			break;

		case 1:
			//found target ui
			scanner.SetActive (true);
			startBoardSetup.SetActive (true);

			scannerBG.color = new Color32 (64, 255, 0, 100);
			break;

		case 2:
			//board creation ui..pinch zoom style or slider style
			scaler.SetActive(true);
			lockInButton.SetActive (true);
			break;
		}
	}

	public void beginToBuildBoard(){

		changeUIStyle (2);
		_buildingBoard = true;

		GameObject boardStart = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube), Vector3.zero, Quaternion.identity) as GameObject;
		boardStart.transform.parent = _boardTarget.transform.parent.transform;
		boardStart.transform.localPosition = Vector3.zero;
		boardStart.transform.localEulerAngles = Vector3.zero;
		boardStart.transform.localScale = new Vector3 (1, 0.3f, 1);
	}

	public void lockInBoardPoint ()
	{
		
	}

	bool _foundTarget;
	bool _buildingBoard;
	bool _lockedIn;
	public BoardTarget _boardTarget;

	public GameObject scanner;
	public GameObject scaler;

	public Text scannerMessage;
	public GameObject startBoardSetup;
	public GameObject lockInButton;
	public UnityEngine.UI.Image scannerBG;

	Transform _worldCenter;
}


//NOTES
/*Find target
 * Lock in for world center
 * Build board
 * 		spawn cube at target center - check
 * 		use pinch zoom to scale width and length of board to fit on table
 * 		lock in to complete board
 * When the board is completed; 
 * 		have a controller to control the positioning of the board
 * 		lock in size and world position
 * 		target can move around but board stays in place
 * 		do math to calculate offset of target from board world position so it stays in the same place even if target is moved
 * 			board will still disapear when target lost
*/