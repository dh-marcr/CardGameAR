using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class StarePointController : NetworkBehaviour
{

	void Start ()
	{
	
		resetPoints ();
		initializing = true;
	}

	public void resetPoints ()
	{
		loading.fillAmount = 0;
		tail.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0.1f, 0);
		BG.color = new Color (BG.color.r, BG.color.g, BG.color.b, 0);
		icon.color = new Color (icon.color.r, icon.color.g, icon.color.b, 0);
	}

	void Update ()
	{

		//turn to camera
		Vector3 lookRotation = new Vector3 (Camera.main.transform.parent.position.x, transform.position.y, Camera.main.transform.parent.position.z);
		transform.LookAt (lookRotation);

		if (!initializing)
			return;

		if (Input.GetKeyDown (KeyCode.S)) {
			show ();
		}
	
		if (staring) {

			fill += 0.5f * Time.deltaTime;

			if (loading.fillAmount >= 1) {
				RpcTriggerStaredAt ();
				loading.fillAmount = 0;
				fill = 0;
				staring = false;
			}
		} else if (!staring && loading.fillAmount != 0) {
			staring = false;
			loading.fillAmount = 0;
			fill = 0;
		}
	}

	void OnChangeLoading(float in_value){

		loading.fillAmount = in_value;
	}

	[ClientRpc]
	void RpcTriggerStaredAt ()
	{

		Debug.Log ("Stared at complete");

		initializing = false;

		tail.color = new Color32 (30, 84, 16, 255);
		BG.color = new Color32 (110, 247, 78, 255);
		icon.color = new Color32 (110, 247, 78, 255);
	}

	public void show ()
	{

		switch (showStep) {

		case 0:
			LeanTween.value (gameObject, 0, 1.5f, 1.5f).setOnUpdate ((val) => tail.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0.1f, val)).setOnComplete (show);
			break;

		case 1:
			LeanTween.value (gameObject, 0, 1, 1).setOnUpdate ((val) => BG.color = new Color (BG.color.r, BG.color.g, BG.color.b, val)).setOnComplete (show);
			break;

		case 2:
			LeanTween.value (gameObject, 0, 1, 1).setOnUpdate ((val) => icon.color = new Color (icon.color.r, icon.color.g, icon.color.b, val)).setOnComplete (show);
			break;

		case 3:
			showStep = 0;
			return;
			break;
		}

		showStep++;
	}

	public void hide ()
	{

		switch (hideStep) {

		case 0:
			LeanTween.value (gameObject, 1.5f, 0, 1.5f).setOnUpdate ((val) => tail.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0.1f, val)).setOnComplete (show);
			break;

		case 1:
			LeanTween.value (gameObject, 1, 0, 1).setOnUpdate ((val) => BG.color = new Color (BG.color.r, BG.color.g, BG.color.b, val)).setOnComplete (show);
			break;

		case 2:
			LeanTween.value (gameObject, 1, 0, 1).setOnUpdate ((val) => icon.color = new Color (icon.color.r, icon.color.g, icon.color.b, val)).setOnComplete (show);
			break;

		case 3:
			hideStep = 0;
			return;
			break;
		}

		hideStep++;
	}

	int showStep = 0;
	int hideStep = 0;

	public Image tail;
	public Image BG;
	public Image icon;
	public Image loading;

	[SyncVar(hook = "OnChangeLoading")]
	public float fill;

	//[HideInInspector]
	public bool staring;

	[HideInInspector]
	public bool initializing;
}
