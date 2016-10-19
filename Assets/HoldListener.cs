using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class HoldListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	void Update () {
	
		float timeSinceDown = Time.time - _downAt;
		float timeSincePreviousDown = Time.time - _previousDownAt;
		float timeSinceUp = Time.time - _upAt;

		if (_isDown && timeSinceDown >= _holdDelay)
		{
			//Debug.Log ("HOLD");
			eventCall.Invoke();
			ignoreNextClick = true;
		}
	}

	public void OnPointerDown (PointerEventData eventData)
	{

		_isDown = true;
		_previousDownAt = _downAt;
		_downAt = Time.time;
	}

	public void OnPointerUp (PointerEventData eventData)
	{

		_isDown = false;
		_wasDown = true;
		_upAt = Time.time;
	}

	public UnityEvent eventCall;

	private float _downAt;
	private float _previousDownAt;
	private float _upAt;

	private bool _isDown = false;
	private bool _wasDown = false;
	private bool ignoreNextClick = false;

	static float _doubleClickThreshold = 0.4f;
	static float _holdDelay = 0.1f;

	static float waitThreshold = 0.01f;
	private float waitTime;
}
