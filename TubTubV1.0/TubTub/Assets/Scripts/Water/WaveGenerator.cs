//Script generates geometry and sets the vertices to follow a sin curve which is modified over time with a timer.

using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

	//Reference Variables
	MeshFilter mf_MeshFilter;
	Mesh ms_Mesh;

	//Public variables used to affect the wave
	public int n_NumOfVerts = 40;
	int n_NumOfTriangles; //value changes depending on value of n_NumOfVerts ( = n_NumOfVerts - 2)
	public float f_WaveSpeed = .2f;
	public float f_WaveHeight = .2f;
	public float f_Length = 5.0f;
	public float f_Width = 120.0f;
	public float f_WaveSmoothness = 3.0f;

	//Arrays used to store values for ms_Mesh being created
	Vector3[] vec3_Verts;
	Vector3[] vec3_VertPos;
	int[] n_TriangleInts;
    Vector2[] vec2_Uvs;

	//used for moving along sin curve at consistent rate
	public float f_Timer;


	void Start () 
	{
		//Set number of triangles hte number of vertices - 2
		n_NumOfTriangles = n_NumOfVerts - 2;
		//Reference the meshfilter component
		mf_MeshFilter = GetComponent<MeshFilter>();
		if (mf_MeshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}
		//get the current sharedmesh for mf_MeshFilter component(which will be null) and create a new ms_Mesh for it. Then clear the ms_Mesh.
		ms_Mesh = mf_MeshFilter.sharedMesh;
		if (ms_Mesh == null){
			mf_MeshFilter.mesh = new Mesh();
			ms_Mesh = mf_MeshFilter.sharedMesh;
		}
		ms_Mesh.Clear();

		//Caclulate the vertices positions and store them into the array vec3_Verts
		vec3_Verts = new Vector3[n_NumOfVerts];
		for (int i = 0; i < n_NumOfVerts; i++)
		{
			int x = i/2;
			int xx = i%2;
			vec3_Verts[i] = (new Vector3 (f_Length/(n_NumOfVerts/2) * x, 0, (xx) * f_Width));
		}

		//Create an array (vec3_VertPos) that will be used the create the vertices for the ms_Mesh created above
		vec3_VertPos = new Vector3[n_NumOfTriangles * 3];
		//Calculate the order to assign the vertices (Clockwise) and then create the vertices
		int count = 0;
		for (int i = 0; i < n_NumOfTriangles; i=i+2)
		{
			Vector3[] tempVerts = new Vector3[6];
			tempVerts[0] = vec3_Verts[i];
			tempVerts[1] = vec3_Verts[i+1];
			tempVerts[2] = vec3_Verts[i + 3];
			tempVerts[3] = vec3_Verts[i];
			tempVerts[4] = vec3_Verts[i + 3];
			tempVerts[5] = vec3_Verts[i + 2];
			for (int x = 0; x < 6; x++)
			{
				vec3_VertPos[(count * 6) + x] = tempVerts[x];
			}
			count++;
		}
		ms_Mesh.vertices = vec3_VertPos;

		//Create an array (n_TriangleInts) to store the values for the meshes triangles and then assign to meshes triangles
		n_TriangleInts = new int[n_NumOfTriangles * 3];
		for (int i = 0; i < n_NumOfTriangles * 3; i++)
		{
			n_TriangleInts[i] = i;
		}
		ms_Mesh.triangles = n_TriangleInts;

        //Set the vec2_Uvs
        vec2_Uvs = new Vector2[vec3_VertPos.Length];
        for (int i = 0; i < vec3_VertPos.Length; i++)
        {
            vec2_Uvs[i] = (new Vector2(vec3_VertPos[i].x, vec3_VertPos[i].z));
        }
        ms_Mesh.uv = vec2_Uvs;

		//Recaclulate normals, bounds, and optimize
		ms_Mesh.RecalculateNormals();
		ms_Mesh.RecalculateBounds();
		ms_Mesh.Optimize();

		//Set the ms_Mesh colliders shared ms_Mesh to match the ms_Mesh that was just created above
		//MeshCollider col = transform.GetComponent (typeof(MeshCollider)) as MeshCollider;
		//col.sharedMesh = null;
		//col.sharedMesh = ms_Mesh;
	}

	void Update () {
		//Use Time.deltaTime to move the wave consistently along a sin curve. This sin curve is affected by the variables: f_WaveSpeed, f_WaveSmoothness, f_WaveHeight.
		f_Timer += Time.deltaTime;
		for (int i = 0; i < vec3_VertPos.Length; i++)
		{
			//vec3_VertPos[i] = new Vector3(vec3_VertPos[i].x,(Mathf.Sin((vec3_VertPos[i].x + transform.position.x + (f_Timer * f_WaveSpeed)) / f_WaveSmoothness)) * f_WaveHeight,vec3_VertPos[i].z);
            vec3_VertPos[i] = new Vector3(vec3_VertPos[i].x, (Mathf.Sin((vec3_VertPos[i].x + (f_Timer * f_WaveSpeed)) / f_WaveSmoothness)) * f_WaveHeight, vec3_VertPos[i].z);
		}
		refreshMesh();
	}

	//Reassign vertices and recacluate the normals,bounds, and optimize. Then set the new ms_Mesh to the meshCollider's shared ms_Mesh.
	void refreshMesh()
	{
		ms_Mesh.vertices = vec3_VertPos;
		ms_Mesh.RecalculateNormals();
		ms_Mesh.RecalculateBounds();
		ms_Mesh.Optimize();

		//MeshCollider col = transform.GetComponent (typeof(MeshCollider)) as MeshCollider;
		//col.sharedMesh = null;
		//col.sharedMesh = ms_Mesh;
	}

}