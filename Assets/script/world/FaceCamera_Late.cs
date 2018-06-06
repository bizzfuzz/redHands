﻿//	CameraFacing.cs 
//	original by Neil Carter (NCarter)
//	modified by Hayden Scott-Baron (Dock) - http://starfruitgames.com
//  allows specified orientation axis


using UnityEngine;
using System.Collections;

public class FaceCamera_Late : MonoBehaviour
{
	Camera referenceCamera;

	public enum Axis {up, down, left, right, forward, back};
	public bool reverseFace = false; 
	public Axis axis = Axis.up; 
	Vector3 targetPos;
	Vector3 targetOrientation;

	// return a direction based upon chosen axis
	public Vector3 GetAxis (Axis refAxis)
	{
		switch (refAxis)
		{
		case Axis.down:
			return Vector3.down; 
		case Axis.forward:
			return Vector3.forward; 
		case Axis.back:
			return Vector3.back; 
		case Axis.left:
			return Vector3.left; 
		case Axis.right:
			return Vector3.right; 
		}

		// default is Vector3.up
		return Vector3.up; 		
	}

	void  Awake ()
	{
		// if no camera referenced, grab the main camera
		if (!referenceCamera)
			referenceCamera = Camera.main; 
	}

	void  LateUpdate ()
	{
		// rotates the object relative to the camera
		if (referenceCamera != null)
		{
			targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back);
			targetOrientation = referenceCamera.transform.rotation * GetAxis (axis);
			transform.LookAt (targetPos, targetOrientation);
		} 
		else
		{
			referenceCamera = Camera.main; 
		}
	}
}