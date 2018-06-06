using System.Collections;
using UnityEngine;

public class Holdable : MonoBehaviour
{
	Transform cam;
	Transform hold;
	//Transform holdfar;
	Thief player;
	[SerializeField] bool grabbed;
	int movespeed = 100;
	[SerializeField] int throwspeed = 10000;
	float holdrange;
	Rigidbody rb;
	float throwdur = 1f;
	float throwtimer;
	bool applythrow = false;
	//NoThrough nt;

	public void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Thief> ();
		hold = player.hold;
		//holdfar = player.holdfar;
		cam = player.cam;
		holdrange = player.holdrange;
		rb = GetComponent<Rigidbody> ();
		//lev = GetComponent<Levitate> ();
		//nt = GetComponent<NoThrough> ();
	}
	
	void Update ()
	{
		if(grabbed)
		{
			transform.position = Vector3.MoveTowards(transform.position, hold.position, movespeed*Time.deltaTime);
			if ((transform.position-hold.position).sqrMagnitude < holdrange) 
			{
				player.canthrow = true;
				//lev.enabled = true;
			}
		}
		else if(applythrow)
		{
			throwtimer -= Time.deltaTime;
			if (throwtimer < 0f)
				applythrow = false;
		}
	}

	public void toss()
	{
		//print ("throw");
		grabbed = false;
		rb.useGravity = true;
		rb.AddForce (cam.forward * throwspeed);
		applythrow = true;
		throwtimer = throwdur;
	}

	public void grab()
	{
		grabbed = true;
		rb.useGravity = false;
	}
}
