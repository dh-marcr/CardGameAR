using UnityEngine;
using System.Collections;

public class DebugUtils : MonoBehaviour {

	static public void AddToLog(string in_string)
	{
		if (debugLog == null)
		{
			debugLog = "";
		}

		debugLog += in_string + "\n";
	}

	static public string outputLog(bool in_clearLog=true)
	{
		string tmp = debugLog;
		if (in_clearLog)
		{
			time += Time.deltaTime;

			if (time > 4) {
				debugLog = "";
				time = 0;
			}
		}
		return tmp;
	}

	static float time;
	static private string debugLog;
}
