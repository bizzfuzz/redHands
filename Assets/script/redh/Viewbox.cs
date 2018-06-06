using System.Collections;
using UnityEngine;

public class Viewbox : MonoBehaviour 
{
	Ai host;
	int player;
	Vector3 dir;
	Transform playerobj;
	RaycastHit hit;
	Transform caststart;
	bool check=false;
	float checktime=-1f;
	float checkwait=.5f;
	[SerializeField] bool npc=false;

	void Start () 
	{
		host = transform.parent.GetComponent<Ai> ();
		player = LayerMask.NameToLayer ("player");
		playerobj = GameObject.FindGameObjectWithTag ("Player").transform.FindChild ("fpcontrol");
		caststart = transform.parent.FindChild ("sensor").FindChild ("c").transform;
	}

	void Update()
	{
		if (check && checktime < 0f) {
			sight ();
			checktime = checkwait;
		}
		else if (checktime > 0f)
			checktime -= Time.deltaTime;
	}

	void sight()
	{
		if (host.state == estate.chase || host.state == estate.attack)
			return;
		dir = (playerobj.position - caststart.position).normalized;//direction to target from transform
		dir.y = 0;
		if (Physics.Raycast (caststart.position, dir, out hit, Mathf.Infinity))// && hit.transform.gameObject.layer==player) 
		{
			Debug.DrawLine (caststart.position, hit.point,Color.white, 10);
			//print (hit.transform);
			if (hit.transform.gameObject.layer == player)
				host.spotplayer ();
			else
				host.loseplayer ();
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer==player)
		{
			check = true;
			host.spotplayer ();
			//print ("pl seen");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.layer==player)
		{
			check = false;
			if (npc)
				host.loseplayer ();
		}
	}
}
