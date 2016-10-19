using UnityEngine;
using UnityEngine.Networking;

public class MyPlayerController : NetworkBehaviour
{

	public override void OnStartLocalPlayer ()
	{
		GetComponent<MeshRenderer> ().material.color = Color.blue;

		FindObjectOfType<InputControl> ().playerController = this;
	}

	void Update ()
	{
		#if UNITY_EDITOR
		if (!isLocalPlayer)
			return;

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {

			CmdFire ();
		}
		#endif
	}

	//#if !UNITY_EDITOR
	public void moveForward ()
	{
		transform.Translate (0, 0, Time.deltaTime * 3);
	}

	public void turnLeft ()
	{
		transform.Rotate (0, Time.deltaTime * -150, 0);
	}

	public void turnRight ()
	{
		transform.Rotate (0, Time.deltaTime * 150, 0);
	}

	//#endif

	[Command]
	public void CmdFire ()
	{
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			             bulletPrefab,
			             bulletSpawn.position,
			             bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn (bullet);

		// Destroy the bullet after 2 seconds
		Destroy (bullet, 2.0f);
	}

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
}

