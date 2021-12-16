using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

	public float restD;
	
	// should set this in the creator - added below for the github repo
	private float stiffness;
	public GameObject p1;
	public GameObject p2;

	public bool drawMe = true;
	private Vector3 diff;
	private float d;

	// adding for stiffness adjustment in the inspector
	private ClothCreator clothCreator;

	public void Solve()
	{
		diff =  p1.transform.position - p2.transform.position;
		d = diff.magnitude;

		float difference = (restD - d) / d;
		
		float im1 = 1 / p1.GetComponent<PointMass>().mass;
		float im2 = 1 / p2.GetComponent<PointMass>().mass;
		//discord assistance here
		float scalarP1 = (im1 / (im1 + im2)) * stiffness;
		float scalarP2 = stiffness - scalarP1;

		// Push/pull based on mass -
		p1.transform.position = p1.transform.position + diff * scalarP1 * difference;
		p2.transform.position = p2.transform.position - diff * scalarP2 * difference;

	}

	private void Awake()
	{
		clothCreator = GameObject.FindGameObjectWithTag("Cloth Creator").GetComponent<ClothCreator>();
	}

	void Start ()
	{
		stiffness = clothCreator.stiffness;
	}


	void Update () 
	{
		if (drawMe) 
		{
			Debug.DrawLine (p1.transform.position, p2.transform.position, Color.blue, Time.deltaTime);
		}
	}
}
