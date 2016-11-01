using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour
{

	void OnTriggerEnter (Collider in_other)
	{

		if (in_other.tag == "StarePoint") {

			in_other.GetComponent<StarePointController> ().staring = true;
		}
	}

	void OnTriggerExit (Collider in_other)
	{

		if (in_other.tag == "StarePoint") {

			in_other.GetComponent<StarePointController> ().staring = false;
		}
	}
}
