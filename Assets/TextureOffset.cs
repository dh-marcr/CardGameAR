using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour {

	void Start () {
	
		rend = GetComponent<Renderer> ();
	}
	
	void Update () {
	
		uvOffset = new Vector2 (transform.localScale.x * offsetMultiplier, transform.localScale.z * offsetMultiplier);
		rend.material.mainTextureScale = uvOffset;
	}

	Vector2 uvOffset;
	Renderer rend;

	public float offsetMultiplier = 5;
}
