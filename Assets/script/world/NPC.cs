using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Interactable 
{
	Transform target;
	NavMeshAgent nmagent;
	[SerializeField]float walkradius=50f;
	[SerializeField]float roamtimer;
	List<string> dialog;
	public Transform player;
	float speakrange=100f;
	[SerializeField]bool speaking=false;
	public Quest job;
	Speach speach;
	public Transform collectables;
	public Rng rng;

	public void Awake()
	{
		//base.Start ();
		//target = GameObject.Find ("cactus").transform;
		nmagent = GetComponent<NavMeshAgent> ();
		nmagent.destination = pickrndpos ();
		dialog = new List<string> ();
		dialog.Add ("test speak");
		dialog.Add ("test speak 2");
		dialog.Add ("test speak 3");
		rng = GetComponent<Rng> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;

//		job = new Quest (this);
		Dialog dbox = GameObject.Find ("dialog").GetComponent<Dialog> ();
		dbox.speaker = this;
		speach = new Speach (dbox);
		speach.lines = dialog;
		speach.quest = true;
		speach.queststatement = dialog.Count-1;

	}
	public override void Start()
	{
		base.Start ();
		job = new Quest (this);
	}
	void LateUpdate()
	{
		if (!nmagent.pathPending)
		{
			if (nmagent.remainingDistance <= nmagent.stoppingDistance)
			{
				if (!nmagent.hasPath || nmagent.velocity.sqrMagnitude == 0f)
				{
					nmagent.destination=pickrndpos ();
				}
			}
		}
		if(speaking && (player.position-transform.position).sqrMagnitude>speakrange)
		{
			//Debug.Log (transform+": "+(player.position-transform.position).sqrMagnitude);
			nmagent.destination = pickrndpos ();
			nmagent.isStopped = false;
			speaking = false;
			speach.end ();
		}
	}
	Vector3 pickrndpos()
	{
		Vector3 randomDirection = Random.insideUnitSphere * walkradius;
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, walkradius, 1);
		return hit.position;
	}
	public override void interact ()
	{
		//Debug.Log ("npc");
		entertalk ();
	}
	void entertalk()
	{
		transform.LookAt (player);
		speaking = true;
		nmagent.isStopped=true;
		speach.shownext ();
		//Debug.Log (transform+" speaking");
	}
}
