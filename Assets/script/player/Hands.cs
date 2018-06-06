using System.Collections;
using UnityEngine;

public class Hands : MonoBehaviour
{
	Transform rmesh;
	Transform lmesh;
	[SerializeField]Transform[] lholders;
	[SerializeField]Transform[] rholders;
	Transform loadout;
	public Weapon rweapon;
	public Weapon lweapon;
	public Transform lhand;
	public Transform rhand;
	[SerializeField]bool rtoggled=false;
	[SerializeField]bool ltoggled=false;
	Transform rwepwrap;
	Transform lwepwrap;
	[SerializeField]Transform armmesh;
	Animator anim;
	public FPSCtrl ctrl;//set by fpsctrl
	float timer;
	float wait=0.01f;
	bool b2double;

	void Start()
	{
		anim = armmesh.GetComponent<Animator> ();
		loadout = transform.FindChild ("loadout");
	}
	void Update()
	{
		if(b2double)
		{
			timer -= Time.deltaTime;
			if(timer<=0f)
			{
				anim.SetBool ("twohands",true);
				b2double = false;
			}
		}
	}
	Weapon getwep(Transform hand)
	{
		if (!hand || hand.childCount == 0)
			return null;
		Weapon wep= hand.GetChild (0).GetComponent<Weapon> ();
		return wep;
	}
	public void refreshammotxt(bool right,int mag,int ammo)//for all funcs right = right hand when true, false=left
	{
		//ctrl.refreahammotxt (right,mag,ammo);
	}
	public void hold(bool right)//called by refresh
	{
		//Debug.Log ("hold: "+!rtoggled+" "+right+" "+rhand.childCount);
		if(!(rtoggled) && right && rhand.childCount > 0)
		{
			rweapon.set=true;
			rweapon.right = true;
			rweapon.hands = this;
			rtoggled = true;//prevents update assingning more than once
			rweapon.gameObject.SetActive (true);//weapons disabled in loadout prevent drawing them
			rmesh = rwepwrap.FindChild ("mesh");
			//Debug.Log (rmesh+" | "+rholders+" | "+rweapon);
			ctrl.rshot = rweapon.shot;
			//Debug.Log ("equip "+rwepwrap+" > "+right);
			if (rweapon.fist)//no weapon mesh to hold
				return;
			rmesh.SetParent (rholders[rweapon.holder]);
			rmesh.localRotation = Quaternion.identity;
			rmesh.localPosition = Vector3.zero;
		}
		else if(!ltoggled && lhand.childCount > 0)
		{
			lweapon.set=true;
			lweapon.right = false;
			lweapon.hands = this;
			ltoggled = true;
			lweapon.gameObject.SetActive (true);
			lmesh = lwepwrap.FindChild ("mesh");
			//Debug.Log (lmesh+" | "+lholders+" | "+lweapon);
			ctrl.lshot = lweapon.shot;
			//Debug.Log ("equip "+lwepwrap+" > "+right);
			if (lweapon.fist)//no weapon mesh to hold
				return;
			lmesh.SetParent (lholders[lweapon.holder]);//put in hand
			lmesh.localRotation = Quaternion.identity;
			lmesh.localPosition = Vector3.zero;
		}
		//Debug.Log ("equip "+wep+" > "+right);
	}
	public void reload(bool right)
	{
		if(right)
			anim.SetTrigger ("rreload");
		else
		{
			anim.SetTrigger ("lreload");
			if(lweapon.twohands) 
			{
				timer = wait;
				b2double = true;
			}
		}
	}
	public void equip(bool right,Transform wep)//moves wep obj from loadout to hand object
	{
		Transform par;
		Weapon w = wep.GetComponent<Weapon> ();
		bool two = w.twohands;
		if(two)
			par = lhand;
		else if (right)
		{
			par = rhand;
		}
		else
		{
			par = lhand;
		}
		//two handed weapon already equipped or to equip, unequip both hands (two handed wep guaranteed parent to left hand)
		if(two)
		{
			unequip (true);
			unequip (false);
		}
		else if(lweapon && lweapon.twohands)
		{
			//Debug.Log ("in | "+!w.twohands);
			unequip (true);
			unequip (false);
			anim.SetBool ("twohands", false);
		}
		else // remove wep already equipped
			unequip (right);//send gun in hand to loadout

		if (two)
			lwepwrap = wep;
		else if (right) //done after so current wep can be unequipped
		{
			rwepwrap = wep;
		}
		else
		{
			lwepwrap = wep;
		}
		wep.SetParent (par);//move wrapper from loadout to hand
		refresh ();//put new
	}
	void refresh()//called on weapon equip
	{
		rweapon = getwep (rhand);
		lweapon = getwep (lhand);
		if (!(lweapon == null && rweapon == null)) //holding something
		{
			/*if (lweapon && lweapon.twohands) {
				anim.SetBool ("twohands",true);
			}
			else
				anim.SetBool ("weaponon", true);*/
			if(lweapon)
			{
				hold (false);
			}
			if(rweapon)
			{
				hold (true);
			}
		}
	}
	public void attack(bool right)
	{
		if (right && rweapon) 
		{
			rweapon.attack ();
			if (rweapon.fist)
				anim.SetTrigger ("rightatt");
			/*else
				anim.SetTrigger ("rshot");//stopped swinging while shooting*/
		}
		else if(!right && lweapon)
		{
			lweapon.attack ();
			if (lweapon.fist)
				anim.SetTrigger ("leftatt");
			/*else
				anim.SetTrigger ("lshot");*/
		}
	}
	public void run(bool running)
	{
		anim.SetBool ("run",running);
	}
	public void grapple(bool grappling)
	{
		anim.SetBool ("grappling",grappling);
	}
	public void tele(bool use)
	{
		anim.SetBool ("tele",use);
	}
	public void unequip(bool right)
	{
		if(right) 
		{
			if(rhand.childCount>0) {
				if(!rweapon.fist)
					rmesh.SetParent (rwepwrap);//return mesh to loadout from hand
				rwepwrap.gameObject.SetActive (false);
				rwepwrap.SetParent (loadout);
				rweapon = null;
				rtoggled = false;
			}
		}
		else
		{
			if(lhand.childCount>0) {
				if(!lweapon.fist)
					lmesh.SetParent (lwepwrap);//return mesh to loadout from hand
				lwepwrap.gameObject.SetActive (false);
				lwepwrap.SetParent (loadout);
				lweapon = null;
				ltoggled = false;
			}
		}
	}
}
