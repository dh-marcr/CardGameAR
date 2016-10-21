using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Debugger : MonoBehaviour {

	void Update () {
	
		debugText.text = DebugUtils.outputLog(true);
	}

	public Text debugText;
}
