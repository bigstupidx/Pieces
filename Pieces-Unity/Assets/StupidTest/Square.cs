using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {
	public tk2dCamera managerCamera;

	// Use this for initialization
	void Start () {
		CreateSquare();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateSquare() {
		GameObject square = new GameObject("Square");
		Rect screenExtents = managerCamera.NativeScreenExtents;
		square.transform.position = new Vector3(screenExtents.width / 2f, screenExtents.height / 2f, 0);

		MeshFilter meshFilter = square.AddComponent<MeshFilter>();
		MeshCollider meshCollider = square.AddComponent<MeshCollider>();

		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;

		Vector3[] verts = new Vector3[4];
		Vector2[] uvs = new Vector2[4];
		int[] tris = new int[6];

		float size = 50;

		verts[0] = new Vector3(0, 0, 0);
		verts[1] = new Vector3(0, size, 0);
		verts[2] = new Vector3(size, size, 0);
		verts[3] = new Vector3(size, 0, 0);

		for (int i = 0; i < verts.Length; i++) {
			Vector3 v = verts[i];
			v -= new Vector3(size / 2f, size / 2f, 0);
			verts[i] = v;
		}	

		uvs[0] = new Vector2(0, 0);
		uvs[1] = new Vector2(0, 1);
		uvs[2] = new Vector2(1, 1);
		uvs[3] = new Vector2(1, 0);

		tris[0] = 0;
		tris[1] = 1;
		tris[2] = 2;
		tris[3] = 2;
		tris[4] = 3;
		tris[5] = 0;

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = mesh;
	}
}
