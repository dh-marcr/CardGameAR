using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour {


	public void fire(){

		playerController.CmdFire ();
	}

	public void moveForward(){

		playerController.moveForward ();
	}

	public void turnLeft(){

		playerController.turnLeft ();
	}

	public void turnRight(){

		playerController.turnRight ();
	}

	public MyPlayerController playerController;
}
