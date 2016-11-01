using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Vuforia;
using Prototype.NetworkLobby;

public class CreatePlayBoard : MonoBehaviour
{

	void Start ()
	{
		changeUIStyle (0);
		changePinchStyle (0);
	}

	public void targetFound (ImageTargetBehaviour in_target)
	{

		DebugUtils.AddToLog ("<color=green>image target </color>" + in_target.name + "<color=green>found</color>");
		_foundTarget = true;
		target = in_target.transform;

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

	void changeUIStyle (int in_style)
	{

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
			scaler.SetActive (true);
			lockInButton.SetActive (true);
			break;
		}
	}

	public void changePinchStyle (int in_style)
	{

		pinchStyle = in_style;
		widthButton.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
		lengthButton.GetComponent<UnityEngine.UI.Image> ().color = Color.white;

		switch (in_style) {

		case 0:
			widthButton.GetComponent<UnityEngine.UI.Image> ().color = Color.grey;
			break;

		case 1:
			lengthButton.GetComponent<UnityEngine.UI.Image> ().color = Color.grey;
			break;
		}
	}

	void Update ()
	{
		if (_buildingBoard) {
			//pinch handling
			Camera cam = Camera.main;
			float distanceBetweenPoints = 0;
			float distanceBetweenStartPoints = 0;

			#if UNITY_EDITOR
			if (Input.GetMouseButtonDown (0)) {
				_isDown = true;
			} else if (Input.GetMouseButtonUp (0)) {
				_isDown = false;
			}

			if (_isDown) {
				if (_touchStartTwo == Vector2.zero) {
					_touchStartOne = new Vector2 (0, 0);
					_touchStartTwo = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

					if (pinchStyle == 0) {
						_scaleAtStart = boardStart.transform.localScale.x;
					} else if (pinchStyle == 1) {
						_scaleAtStart = boardStart.transform.localScale.z;
					}
				}
				distanceBetweenPoints = Vector2.Distance (Vector2.zero, Input.mousePosition);
				distanceBetweenStartPoints = Vector2.Distance (_touchStartOne, _touchStartTwo);
				handlePinchZoom (distanceBetweenStartPoints, distanceBetweenPoints);
			} else {
				clearTouchPoints ();
			}

			#else

			if (Input.touchCount == 2)
			{
				Touch touchZero = Input.GetTouch (0);
				Touch touchOne = Input.GetTouch (1);

			if (_touchStartOne == Vector2.zero)
			{
				_touchStartOne = new Vector2(touchZero.position.x, touchZero.position.y);
				_touchStartTwo = new Vector2(touchOne.position.x, touchOne.position.y);

				if (pinchStyle == 0) {
					_scaleAtStart = boardStart.transform.localScale.x;
				} else if (pinchStyle == 1) {
					_scaleAtStart = boardStart.transform.localScale.z;
				}
			}
				distanceBetweenPoints = Vector3.Distance (touchZero.position, touchOne.position);
				distanceBetweenStartPoints = Vector2.Distance (_touchStartOne, _touchStartTwo);
				handlePinchZoom (distanceBetweenStartPoints, distanceBetweenPoints);
			} else
			{
			clearTouchPoints ();
			}

			#endif
		}
	}

	void handlePinchZoom (float in_touchStartDistance, float in_touchCurrentDistance)
	{

		float pinchDelta = in_touchCurrentDistance - in_touchStartDistance;
		float deltaScale = pinchDelta / maxPinchZoomDistance;
		float rawPercent = _scaleAtStart + deltaScale;
		float newScale = Mathf.Clamp (rawPercent, 0.02f, int.MaxValue);

		switch (pinchStyle) {

		//handle width pinch
		case 0:
			boardStart.transform.localScale = new Vector3 (newScale, 0.3f, boardStart.transform.localScale.z);
			break;

		//handle length pinch
		case 1:
			boardStart.transform.localScale = new Vector3 (boardStart.transform.localScale.x, 0.3f, newScale);
			break;
		}
	}

	void clearTouchPoints ()
	{

		_touchStartOne = Vector2.zero;
		_touchStartTwo = Vector2.zero;
	}

	public void beginToBuildBoard ()
	{

		changeUIStyle (2);
		_buildingBoard = true;

		boardStart = (GameObject)Instantiate (Resources.Load("TileBoard"), Vector3.zero, Quaternion.identity) as GameObject;
		boardStart.transform.parent = target.transform.transform;
		boardStart.transform.localPosition = Vector3.zero;
		boardStart.transform.localEulerAngles = Vector3.zero;
		boardStart.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
	}

	public void lockInBoardPoint ()
	{
		LobbyManager lb = FindObjectOfType<LobbyManager> ();
		lb.gamePlayerPrefab.GetComponent<GameInitialization> ().boardScale = boardStart.transform.localScale;

		LobbyMainMenu lbmm = FindObjectOfType<LobbyMainMenu> ();
		lbmm.OnClickCreateMatchmakingGame ();
	}

	public Transform target;

	//scaling
	bool _isDown;
	GameObject boardStart;
	Vector2 _touchStartOne = Vector2.zero;
	Vector2 _touchStartTwo = Vector2.zero;
	float _scaleAtStart = 1;
	static float maxPinchZoomDistance = 1000;

	bool _foundTarget;
	bool _buildingBoard;
	bool _lockedIn;

	public GameObject scanner;
	public GameObject scaler;

	int pinchStyle;
	Color normalButtonColor = new Color (255, 255, 255, 255);
	Color pressedButtonColor = new Color (200, 200, 200, 255);
	public Button widthButton;
	public Button lengthButton;

	public Text scannerMessage;
	public GameObject startBoardSetup;
	public GameObject lockInButton;
	public UnityEngine.UI.Image scannerBG;

	Transform _worldCenter;

	public GameObject tileBoardPrefab;
}


//NOTES
/*Find target
 * Lock in for world center
 * Build board
 * 		spawn cube at target center - check
 * 		use pinch zoom to scale width and length of board to fit on table - check
 * 		lock in to complete board
 * When the board is completed; 
 * 		have a controller to control the positioning of the board
 * 		lock in size and world position
 * 		target can move around but board stays in place
 * 		do math to calculate offset of target from board world position so it stays in the same place even if target is moved
 * 			board will still disapear when target lost
*/