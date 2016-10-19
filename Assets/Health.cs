using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;
		
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			currentHealth = maxHealth;
			RpcRespawn ();
		}
	}

	void OnChangeHealth(int in_currentHealth){

		healthbar.sizeDelta = new Vector2 (in_currentHealth, healthbar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			// move back to zero location
			transform.position = Vector3.zero;
			transform.eulerAngles = Vector3.zero;
		}
	}

	public RectTransform healthbar;

	public const int maxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
}
