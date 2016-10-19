using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Vuforia;

public class CreatePlayBoard : MonoBehaviour {

	void Update(){

		if(Input.GetKeyDown(KeyCode.R)){

			SceneManager.LoadScene("BoardCreationScene", LoadSceneMode.Single);
		}
	}

	public void targetFound(ImageTargetBehaviour in_target){

		Debug.Log ("<color=green>image target </color>" + in_target.name + "<color=green>found</color>");
	}

	public void targetLost(ImageTargetBehaviour in_target){
		
		Debug.Log ("<color=red>image target </color>" + in_target.name + "<color=red>lost</color>");
	}
}
