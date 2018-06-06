using System.Collections;
using UnityEngine;

public class Lightbulb : MonoBehaviour
{
	Transform target;
	Thief player;
	public float range=10f;
	Vector3 dir;
	RaycastHit hit;
	int playerlayer;
	public bool activelight=false;
	Vector3 voff = new Vector3(0,1,0);
	public GameObject lightobj;
	[SerializeField] float rayrange = 100f;
	float playerdist;

	void Awake()
	{
		lightobj = transform.GetChild (0).gameObject;
	}

	void Start () 
	{
		Transform playerwrap = GameObject.FindGameObjectWithTag ("Player").transform;
		player = playerwrap.GetComponent < Thief> ();
		target = playerwrap.transform.FindChild ("fpcontrol");
		playerlayer = LayerMask.NameToLayer ("player");
		//light = transform.GetChild (0).gameObject;
		StartCoroutine ("work", .5f);
	}

	void OnEnable()
	{
		StartCoroutine ("work", .5f);
	}

	IEnumerator work(float delay)
	{
		while(true)
		{
			yield return new WaitForSeconds (delay);
			playerdist = (transform.position-target.position).sqrMagnitude;
			if(lightobj.activeSelf && playerdist<rayrange)
				shine();
			else if(activelight)
			{
				unlightplayer ();
			}
		}
	}

	void shine()
	{
		//print ("shine");
		dir = ((target.position + voff) - transform.position).normalized;//direction to target from transform
		//dir.y = 0;
		if (Physics.Raycast (transform.position, dir, out hit, range))// && hit.transform.gameObject.layer==player) 
		{
			Debug.DrawLine (transform.position, hit.point,Color.white, 10);
			//print (hit.transform);
			if(hit.transform.gameObject.layer==playerlayer) 
			{
				player.lit = true;
				activelight = true;
			}
		}
	}

	public void unlightplayer()
	{
		player.lit = false;
		activelight = false;
	}

	public void off()
	{
		if(activelight)
		{
			unlightplayer ();
		}
		lightobj.SetActive (false);
	}

	public void on()
	{
		lightobj.SetActive (true);
	}
}
