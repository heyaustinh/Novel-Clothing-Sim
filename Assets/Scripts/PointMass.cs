﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMass : MonoBehaviour {

	[SerializeField]
	public float mass;

	private float elapsedTime;
	private float extraTime;

	[SerializeField]
	private float timestep;
	private int numChunks;
	private Vector3 nextPos;
	private Vector3 lastPos;
	private Vector3 pinPos;
	private Vector3 vel;
	private Vector3 acc;

	public List<Link> links;
	public bool pinned = false;
	private float tssq;

	void Start () {
		if (timestep <= 0)
			timestep = 1;
		elapsedTime = 0;
		extraTime = 0;
		pinPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		lastPos = transform.position;
		tssq = timestep* timestep;
	}
	

	void Update () {

		elapsedTime = Time.deltaTime;
		elapsedTime += extraTime;

		numChunks = (int) Mathf.Floor(elapsedTime / timestep);
		extraTime = elapsedTime - timestep * numChunks;

		//iterate numchunks times every frame 
		//reduce the chunks if we have a lot
		for (int i = 0; i < numChunks; i++) 
		{

			foreach (var item in links) 
			{
				if(item)
					item.Solve ();	
			}
			
			vel = transform.position - lastPos;
			// damping the Velocity
			vel *= 0.99f;
			nextPos = transform.position + vel + acc * tssq;
			lastPos = transform.position;
			transform.position = nextPos;
			acc = Vector3.zero;

			//detect floor - kinda hacky and dependent on world position rather than interaction with floor
			if (transform.position.y < 0)
				transform.position = new Vector3 (transform.position.x, 0, transform.position.z);

			// don't move pinned nodes
			if(pinned)
				transform.position = pinPos;

		}
	}
	
	//import math utils later
	public void AddForce(Vector3 f)
	{
		acc += f/mass;
	}
		

	void OnCollisionEnter(Collision col)
	{
		ContactPoint cpt = col.contacts [0];
		AddForce(cpt.normal.normalized);
	}


}
