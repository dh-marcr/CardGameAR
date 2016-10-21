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

		changeUIForTargetLost ();
	}

	public void targetFound (ImageTargetBehaviour in_target)
	{

		DebugUtils.AddToLog ("<color=green>image target </color>" + in_target.name + "<color=green>found</color>");

		_boardTarget = in_target.transform.FindChild ("Origin").GetComponent<BoardTarget> ();

		if (!_boardTarget._foundAndLockedIn) {
			changeUIForTargetFound ();
		} else {
			DebugUtils.AddToLog ("<color=blue>Found and locked in that point already. Find another!</color>");
		}
	}

	public void targetLost (ImageTargetBehaviour in_target)
	{
		
		DebugUtils.AddToLog ("<color=red>image target </color>" + in_target.name + "<color=red>lost</color>");
		changeUIForTargetLost ();
	}

	void changeUIForTargetFound ()
	{
		scannerBG.color = new Color32 (16, 210, 0, 100);
		lockInButton.SetActive (true);
		scannerMessage.text = "Found Target";
	}

	void changeUIForTargetLost(){
		scannerBG.color = new Color32 (126, 126, 126, 100);
		lockInButton.SetActive (false);
		scannerMessage.text = "Look for corner target";
	}

	public void lockInBoardPoint ()
	{
		GameObject newOrigin = (GameObject)Instantiate (new GameObject("NewOrigin"), _boardTarget.transform.position, Quaternion.identity) as GameObject;

		if (_boardTarget.creationType == BoardCreationType.corner && !cornerTransforms.Contains (newOrigin.transform)) {

			cornerTransforms.Add (newOrigin.transform);

		} else {

			DebugUtils.AddToLog ("<color=red>You have locked that corner in already, look for another</color>");
		}

		if (!boardCreated) {
			if (_boardTarget.creationType == BoardCreationType.worldCenter && cornerTransforms.Count == 4) {

				_worldCenter = newOrigin.transform;
			
				cornerTransformsSorted = cornerTransforms.ToArray ();
				angleOfCornerTransforms = new float[4];

				for (int i = 0; i < cornerTransformsSorted.Length; i++) {

					angleOfCornerTransforms [i] = Vector3.Angle (_worldCenter.position, cornerTransformsSorted [i].position);
					DebugUtils.AddToLog ("<color=blue>angle : " + angleOfCornerTransforms [i] + "</color>");
				}

				System.Array.Sort (angleOfCornerTransforms, cornerTransformsSorted);

				for (int j = 0; j < cornerTransforms.Count; j++) {

					cornerTransforms [j].position = cornerTransformsSorted [j].position;
				}

				//create the board
				Vector3 boardScale = Vector3.zero;
				float length1 = Vector3.Distance (cornerTransforms [0].position, cornerTransforms [1].position);
				float length2 = Vector3.Distance (cornerTransforms [0].position, cornerTransforms [3].position);

				if (length1 > length2) {
					boardScale = new Vector3 (length2, 5, length1);
				} else if (length2 > length1) {
					boardScale = new Vector3 (length1, 5, length2);
				}

				GameObject newBoard = GameObject.CreatePrimitive (PrimitiveType.Cube);
				newBoard.transform.localScale = boardScale;
				newBoard.transform.parent = _boardTarget.transform;
				newBoard.transform.position = Vector3.zero;

			} else {

				DebugUtils.AddToLog ("<color=red>find all the corners first</color>");
			}
		} else {
			DebugUtils.AddToLog ("<color=red>Board has been built already!!</color>");
		}

		_boardTarget._foundAndLockedIn = true;
		changeUIForTargetLost ();
	}

	void Update(){

	}

	//stuff for scanner
	public BoardTarget _boardTarget;

	bool boardCreated;

	public List<Transform> cornerTransforms = new List<Transform> ();
	public Transform[] cornerTransformsSorted;
	public float[] angleOfCornerTransforms;

	public Text scannerMessage;
	public GameObject lockInButton;
	public UnityEngine.UI.Image scannerBG;

	Transform _worldCenter;
}
