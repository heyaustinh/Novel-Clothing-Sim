using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothCreator : MonoBehaviour {

	[SerializeField] private GameObject pointMass;
	[SerializeField] private GameObject link;

	[SerializeField] private GameObject[] cloth;
	[SerializeField] private GameObject[] links;
	
	[SerializeField] private Vector3 startPos;
	[SerializeField] private float xGap = 2;
	[SerializeField] private float yGap = 2;
	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private bool gravity = true;
	private Vector3[] newVerts;
	[SerializeField] private MeshMaker mm;
	
	//adding to make it more apparent how to handle link stiffness
	
	[SerializeField][Range(0,1f)] public float stiffness;
	
	void Start () {

		cloth = new GameObject[(width) * (height)];
		links = new GameObject[6 * (width) * (height)];
		newVerts = new Vector3[cloth.Length];

		int linkno = 0;

		GameObject clothParent = GameObject.Find("Cloth");
		GameObject linkParent = GameObject.Find("Links");

		for (int j = 0; j < height; j++) {
			for (int i = 0; i < width; i++) {
				cloth [ind (i, j)] = Instantiate (pointMass, 
					startPos + new Vector3 (i * xGap, 0, 0) + new Vector3 (0, 0,j * yGap),
								Quaternion.identity,
								clothParent.transform);
				// X axis links
				if (i > 0) 
				{
					AddLink (linkno, i, j, i - 1, j, linkParent);
					linkno += 1;
				}
				// Y axis links
				if (j > 0) 
				{
					AddLink (linkno, i, j, i, j - 1, linkParent);
					linkno += 1;
				}
				// Pinning
				if (j == 0) 
				{
					if(i==0 || i == width-1)
						cloth [ind (i, j)].GetComponent<PointMass> ().pinned = true;

				}
				
			}
		}

		// Criss cross links (shear)
		for (int i = 0; i < width; i++) 
		{
			for (int j = 1; j < height; j++) 
			{
				if (i % 2 == 0) 
				{
					if(i>0)
					{
						AddLink (linkno, i, j, i - 1, j - 1, linkParent);
						linkno += 1;
					}
					AddLink (linkno, i, j, i + 1, j - 1, linkParent);
					linkno += 1;
				}
				else 
				{
					if (i < width - 1) 
					{
						AddLink (linkno, i, j, i + 1, j - 1, linkParent);
						linkno += 1;
					}
					AddLink (linkno, i, j, i - 1, j - 1, linkParent);
					linkno += 1;
				}
			}
		}

		
		for (int i = 0; i < width - 2; i++) 
		{
			for (int j= 0; j < height -2 ; j++) {
				AddLink (linkno, i, j, i + 2, j, linkParent);
				linkno += 1;
				AddLink (linkno, i, j, i, j + 2, linkParent);
				linkno += 1;
			}
		}

	}
	
	// return index
	int ind(int i, int j)
	{
		return i + (width) * j;
	}


	void Update()
	{
		
		int i = 0;

		foreach (var item in cloth) {
			if(gravity)
				item.GetComponent<PointMass> ().AddForce (new Vector3(0, -9.8f, 0));
			newVerts [i++] = item.transform.position;
		}

		if (Input.GetKeyDown (KeyCode.G)) 
		{
			gravity = !gravity;
		}


		mm.verts = newVerts;
		mm.SetMesh ();
	}

	void AddLink(int linkIndex, int i1, int j1, int i2, int j2, GameObject lp, bool dm = false)
	{
		links [linkIndex] = Instantiate (link, new Vector3 (0, 0, 0), Quaternion.identity, lp.transform);
		Link l = links [linkIndex].GetComponent<Link> ();
		l.p1 = cloth [ind (i1, j1)];
		l.p2 = cloth [ind (i2, j2)];
		l.drawMe = dm;
		l.restD = (l.p1.transform.position - l.p2.transform.position).magnitude;
		cloth [ind (i1, j1)].GetComponent<PointMass> ().links.Add (l);
	}

}
