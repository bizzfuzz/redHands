using System.Collections;
using UnityEngine;

public class Horse : Interactable
{
	Transform player;
	Transform saddle;
	Rigidbody playerrb;
	FPSCtrl plctrl;
	[SerializeField] float spd;
	// Use this for initialization
	override public void Start () 
	{
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		saddle = transform.FindChild ("mount");
		plctrl = player.GetComponent<FPSCtrl> ();
		playerrb = plctrl.rb;
	}
	
	// Update is called once per frame
	override public void interact () 
	{
		player.SetParent (saddle);
		player.localRotation = Quaternion.identity;
		player.localPosition = Vector3.zero;
		playerrb.useGravity = false;
		plctrl.mount = this;
		plctrl.mounted = true;
	}

	public void ride()
	{
		if (Input.GetKey (plctrl.up)) 
		{
			//Debug.Log ("in");
			transform.Translate (Vector3.forward * spd * Time.deltaTime);
			//transform.Translate (transform.forward * spd * Time.deltaTime);
		}
		if (Input.GetKey (plctrl.down)) 
		{
			transform.Translate (Vector3.back * spd * Time.deltaTime);
		}
		if (Input.GetKey (plctrl.left)) 
		{
			transform.Translate (Vector3.left * spd * Time.deltaTime);
		}
		if (Input.GetKey (plctrl.right)) 
		{
			transform.Translate (Vector3.right * spd * Time.deltaTime);
		}

	}
}
