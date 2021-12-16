﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour {

	private Mesh mesh;
	public int xSize, ySize;
	public Vector3[] verts;

	void Awake()
	{
		GenerateMesh();
	}



	public void SetMesh()
	{
		mesh.vertices = verts;
	}

	void GenerateMesh()
	{
		verts = new Vector3[(xSize + 1) * (ySize + 1)];

		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Cloth";

		Vector2[] uv = new Vector2[(xSize+1) * (ySize+1)];

		for (int i = 0, y = 0; y <= ySize; y++) 
		{
			for (int x = 0; x <= xSize; i++, x++ ) 
			{
				verts [i] = new Vector3 (x, y);
				uv [i] = new Vector2 ((float)x / xSize, (float)y / ySize);
			}
		}
		mesh.vertices = verts;
		mesh.uv = uv;
		CalculateTris();
	}

	public void CalculateTris()
	{
		int[] tris = new int[xSize * ySize * 6];
		
		for (int ti = 0, vi = 0,y = 0; y < ySize; y++, vi++)
		{
			for (int x = 0; x < xSize; x++, ti += 6, vi++) 
			{
				tris [ti] = vi;
				tris [ti + 3] = tris [ti + 2] = vi + 1;
				tris [ti + 4] = tris [ti + 1] = vi + xSize + 1;
				tris [ti + 5] = vi + xSize + 2;
			}
		}
		
		mesh.triangles = tris;
		mesh.RecalculateNormals();
	}
	
}
