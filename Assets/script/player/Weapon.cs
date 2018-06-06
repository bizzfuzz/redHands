using System.Collections;
using UnityEngine;

public enum shot
{
	semi,auto
}

public class Weapon : MonoBehaviour {
	[SerializeField]int dmg;
	[SerializeField]int mag;
	[SerializeField]int ammo;
	[SerializeField]int magsize;
	[SerializeField]float range=20f;
	[SerializeField]float cooldown;
	public bool fist=false;
	public bool melee;
	[SerializeField]float timer;
	public bool set;
	public bool right;
	public int holder;
	public Hands hands;
	ParticleSystem flash;
	public bool twohands=false;
	Transform cam;
	public shot shot=shot.semi;
	float reloadtime=0.7f;
	[SerializeField] bool shotgun = false;
	Transform shotgunstart;
	int wrestlers;
	//Recoil recoil;

	void Awake () {
		flash = transform.GetComponentInChildren<ParticleSystem>();
		//Debug.Log ("in");
		cam = Camera.main.transform;
		//Debug.Log ("in: "+cam+" - "+Camera.main);
		shotgunstart = cam.FindChild ("drop");
		wrestlers = LayerMask.NameToLayer ("wrestler");
		//recoil = cam.GetComponent<Recoil> ();
	}
	void Start()
	{
		if (!melee)
			reload ();
	}
	void Update () 
	{
		if(!set)//cooldown between attacks
		{
			timer -= Time.deltaTime;
			if (timer <= 0f) 
			{
				set = true;
			}
		}
	}
	void reload()
	{
		timer = reloadtime;
		set = false;
		mag = Mathf.Min (magsize, ammo);
		ammo -= mag;
		hands.reload (right);
		hands.refreshammotxt (right,mag,ammo);
	}
	public void attack()//blind fire
	{
		if (!set)//cooling
			return;
		timer = cooldown;
		set = false;
		if(!melee)
		{
			if (mag <= 0) {
				reload ();
				return;
			}
			flash.Play ();
			mag--;
			hands.refreshammotxt (right,mag,ammo);
		}
		if (shotgun)
			for(int i=0;i<8;i++)
			{
				Vector3 start = shotgunstart.position + Random.insideUnitSphere * 3;
				start.z = shotgunstart.position.z;
				shoot (start);
			}
		else
			shoot ();
		//recoil.recoilBack ();
	}
	public void shoot()
	{
		RaycastHit hit;
		//Debug.Log (Camera.main);
		if (Physics.Raycast (cam.position, cam.forward, out hit, range)) 
		{
			GameObject temp;//blood or sparks depending on object hit
			//Debug.Log (hit.transform.name);
			Health hp;
			if (hit.transform.gameObject.layer == wrestlers ) 
			{ //hit enemy
				if (temp = Pool.shared.getobj (pooltype.blood)) {
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp.SetActive (true);
				}
				hp = hit.transform.GetComponent<Health> ();
				hp.change (-dmg);
			}
			else
			{ //hit other object
				if (!melee) {//make a bullet hole if gun
					GameObject temp2; 
					if (temp2 = Pool.shared.getobj (pooltype.hole)) 
					{
						temp2.transform.position = hit.point;
						temp2.transform.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);
						temp2.SetActive (true);
					}
				}
				//sparks
				if (temp = Pool.shared.getobj (pooltype.spark)) 
				{
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp.SetActive (true);
				}
			}
		}
	}
	public void shoot(Vector3 startpos)
	{
		RaycastHit hit;
		if (Physics.Raycast (startpos, cam.forward, out hit, range)) 
		{
			GameObject temp;//blood or sparks depending on object hit
			//Debug.Log (hit.transform.name);
			Health hp;
			if (hp = hit.transform.gameObject.GetComponent<Health> ()) 
			{ //hit enemy
				if (temp = Pool.shared.getobj (pooltype.blood)) {
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp.SetActive (true);
				}
				hp.change (-dmg);
			}
			else
			{ //hit other object
				if (!melee) {//make a bullet hole if gun
					GameObject temp2; 
					if (temp2 = Pool.shared.getobj (pooltype.hole)) 
					{
						temp2.transform.position = hit.point;
						temp2.transform.rotation = Quaternion.FromToRotation (Vector3.forward, hit.normal);
						temp2.SetActive (true);
					}
				}
				//sparks
				if (temp = Pool.shared.getobj (pooltype.spark)) 
				{
					temp.transform.position = hit.point;
					temp.transform.rotation = Quaternion.LookRotation (hit.normal);
					temp.SetActive (true);
				}
			}
		}
	}
}