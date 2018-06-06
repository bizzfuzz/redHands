
using UnityEngine;

// Very simple smooth mouselook modifier for the MainCamera in Unity
// by Francis R. Griffiths-Keam - www.runningdimensions.com

[AddComponentMenu("Camera/Simple Smooth Mouse Look ")]
public class FPSCam : MonoBehaviour
{
	Vector2 _mouseAbsolute;
	Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360, 180);
	public Vector2 sensitivity = new Vector2(2, 2);
	public Vector2 smoothing = new Vector2(3, 3);
	public Vector2 targetDirection;
	public Vector2 targetCharacterDirection;

	// Assign this if there's a parent object controlling motion, such as a Character Controller.
	// Yaw rotation will affect this object instead of the camera if set.
	public GameObject characterBody;
	Quaternion camrot;
	Transform player;
	public bool running=true;
	Quaternion targetOrientation;
	Quaternion targetCharacterOrientation;
	public Vector2 mouseDelta;
	string mousex = "Mouse X";
	string mousey = "Mouse Y";

	void Start()
	{
		// Set target direction to the camera's initial orientation.
		targetDirection = transform.localRotation.eulerAngles;
		player = transform.parent;

		// Set target direction for the character body to its inital state.
		//if (characterBody) targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
	}

	void Update()
	{
		if(running)
		{
			// Ensure the cursor is always locked when set

			//Cursor.lockState=CursorLockMode.Locked;

			// Allow the script to clamp based on a desired target value.
			targetOrientation = Quaternion.Euler(targetDirection);
			targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

			// Get raw mouse input for a cleaner reading on more sensitive mice.
			mouseDelta = new Vector2(Input.GetAxisRaw(mousex), Input.GetAxisRaw(mousey)); //use as base for flying pitch?
			/*camrot = Quaternion.Euler (0,mouseDelta.x,0f);
		player.rotation = camrot;*/
			// Scale input against the sensitivity setting and multiply that against the smoothing value.
			mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

			// Interpolate mouse movement over time to apply smoothing delta.
			_smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
			_smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

			// Find the absolute mouse movement value from point zero.
			_mouseAbsolute += _smoothMouse;

			// Clamp and apply the local x value first, so as not to be affected by world transforms.
			if (clampInDegrees.x < 360)
				_mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

			var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
			transform.localRotation = xRotation;

			// Then clamp and apply the global y value.
			if (clampInDegrees.y < 360)
				_mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

			transform.localRotation *= targetOrientation;

			// If there's a character body that acts as a parent to the camera
			if (player)
			{
				var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
				player.localRotation = yRotation;
				player.localRotation *= targetCharacterOrientation;
			}
			else
			{
				var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
				transform.localRotation *= yRotation;
			}
		}
	}
}