using System.Collections;
using UnityEngine;

public enum recoilmove
{
	forward,back,def
}

public class Recoil : MonoBehaviour {

	[SerializeField]float recoilSpeed = 0.2f;    // Speed to move camera
	//KeyCode movek=KeyCode.LeftControl;
	Vector3 ogpos;
	bool done=false;
	[SerializeField]float timer;
	recoilmove move;
	[SerializeField] float snapback=0.01f;

	void Awake()
	{
		ogpos = transform.localPosition;
	}

	void Update () 
	{
		if (done && move==recoilmove.def) 
		{
			transform.localPosition = ogpos;
			done = false;
		}
		else if(move==recoilmove.back)
		{
			timer -= Time.deltaTime;
			if(timer<=0f)
			{
				recoilForward ();
				move = recoilmove.def;
			}
		}
	}

	// Move current weapon to zoomed in position smoothly over time
	IEnumerator MoveToPosition(Vector3 newPosition, float time)
	{
		float elapsedTime = 0;
		var startingPos = transform.position;

		while (elapsedTime < time)
		{
			transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return new WaitForSeconds (0.01f);;
		}
		done = true;
	}

	public void recoilBack()
	{
		timer = snapback;
		move = recoilmove.back;
		// Start coroutine to move the camera up smoothly over time
		Vector3 zoomOutOffset = new Vector3(0f, 0f, -0.5f);
		Vector3 zoomOutWorldPosition = transform.TransformDirection( zoomOutOffset );
		// Move the camera smoothly 
		StartCoroutine(MoveToPosition(transform.position + zoomOutWorldPosition, recoilSpeed));             
	}

	void recoilForward()
	{
		move = recoilmove.forward;
		// Start coroutine to move the camera down smoothly over time
		Vector3 zoomInOffset = new Vector3(0f, 0f, 0.5f);
		Vector3 zoomInWorldPosition = transform.TransformDirection( zoomInOffset );
		// Move the camera smoothly 
		StartCoroutine(MoveToPosition(transform.position + zoomInWorldPosition, recoilSpeed));
	}
}
