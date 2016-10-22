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

	}

	public void targetFound (ImageTargetBehaviour in_target)
	{

		DebugUtils.AddToLog ("<color=green>image target </color>" + in_target.name + "<color=green>found</color>");

		_boardTarget = in_target.transform.FindChild ("Origin").GetComponent<BoardTarget> ();

		if (!_boardTarget) {
		}
	}

	public void targetLost (ImageTargetBehaviour in_target)
	{
		
		DebugUtils.AddToLog ("<color=red>image target </color>" + in_target.name + "<color=red>lost</color>");
	}

	void changeUIStyle(int in_style){

		switch (in_style) {

		case 0:
			//default scanner ui
			break;

		case 1:
			//found target ui
			break;

		case 2:
			//board creation ui..pinch zoom style or slider style
			break;
		}
	}

	public void beginToBuildBoard(){

		GameObject boardStart = (GameObject)Instantiate (GameObject.CreatePrimitive (PrimitiveType.Cube), Vector3.zero, Quaternion.identity) as GameObject;
		boardStart.transform.parent = _boardTarget.transform.parent.transform;
		boardStart.transform.position = Vector3.zero;
		boardStart.transform.localScale = new Vector3 (1, 0.3f, 1);


	}

	public void lockInBoardPoint ()
	{
		
	}

	//stuff for scanner
	public BoardTarget _boardTarget;

	bool boardCreated;

	public Text scannerMessage;
	public GameObject lockInButton;
	public UnityEngine.UI.Image scannerBG;

	Transform _worldCenter;
}


//NOTES
/*Find target
 * Lock in for world center
 * Build board
 * 		spawn cube at target center
 * 		use pinch zoom to scale width and length of board to fit on table
 * 		lock in to complete board
 * Wehn the board is completed; 
 * 		have a controller to control the positioning of the board
 * 		lock in size and world position
 * 		target can move around but board stays in place
 * 		do math to calculate offset of target from board world position so it stays in the same place even if target is moved
 * 			board will still disapear when target lost
*/