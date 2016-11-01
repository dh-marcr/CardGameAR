using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour {

	void Awake(){

		if (instance == null) {
			instance = this;
		}

		DontDestroyOnLoad (gameObject);
	}

	public void loadSceneAdditive(string in_scene){

		SceneManager.LoadScene (in_scene, LoadSceneMode.Additive);
	}

	public void loadScene(string in_scene){

		SceneManager.LoadScene (in_scene, LoadSceneMode.Single);
	}

	void OnDestroy(){

		instance = null;
	}

	static public SceneController instance;
}
