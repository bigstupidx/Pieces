using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeshCreator : MonoBehaviour {
	public tk2dCamera managerCamera;
	public Material material;

	int curBitmask;
	MeshFilter meshFilter;
	MeshRenderer meshRenderer;
	Mesh mesh;
	float baseTileSize = 100;
	float flashScaleAddition = 50;
	Color flashColor = Color.green;

	float curTileSize;
	Color curFlashColor = Color.white;

	// Use this for initialization
	void Start () {
		curTileSize = baseTileSize;

		GameObject obj = new GameObject("Object");
		Vector2 screenSize = managerCamera.NativeResolution;
		obj.transform.position = new Vector3(screenSize.x / 2f, screenSize.y / 2f, 0);

		meshFilter = obj.AddComponent<MeshFilter>();
		meshRenderer = obj.AddComponent<MeshRenderer>();
		meshRenderer.material = material;

		StartCoroutine(StartPieceLoop(60f/128.3f));
		//RedrawWithPieceBitmask(Convert.ToInt32("01111000", 2));
	}
	
	// Update is called once per frame
	void Update () {
		meshRenderer.material.color = curFlashColor;
	}

	IEnumerator StartPieceLoop(float loopTime) {
		while (true) {
			string s = "";
			for (int i = 0; i < 8; i++) s += (UnityEngine.Random.value < 0.5f ? 1 : 0).ToString();
			curBitmask = Convert.ToInt32(s, 2);
			RedrawWithPieceBitmask(curBitmask);
			StartCoroutine(Flash(0.1f));

			yield return new WaitForSeconds(loopTime);
		}
	}

	IEnumerator Flash(float flashTime) {
		float timeOfCall = Time.time;
		curTileSize += flashScaleAddition;
		float deltaTime;

		while (Time.time < timeOfCall + flashTime) {
			deltaTime = Time.time - timeOfCall;

			curTileSize = Mathf.Lerp(baseTileSize + flashScaleAddition, baseTileSize, deltaTime / flashTime);
			curFlashColor = Color.Lerp(flashColor, Color.white, deltaTime / flashTime);

			RedrawWithPieceBitmask(curBitmask);

			yield return null;
		}

		curTileSize = baseTileSize;
		curFlashColor = Color.white;

		RedrawWithPieceBitmask(curBitmask);
	}

	void RedrawWithPieceBitmask(int bitmask) {
		List<Vector3> vertList = new List<Vector3>();

		if ((bitmask & 1 << 0) == 1 << 0) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(-curTileSize / 2f, 0, 0));
			vertList.Add(new Vector3(-curTileSize / 2f, curTileSize / 2f, 0));
		}

		if ((bitmask & 1 << 1) == 1 << 1) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(-curTileSize / 2f, curTileSize / 2f, 0));
			vertList.Add(new Vector3(0, curTileSize / 2f, 0));
		}

		if ((bitmask & 1 << 2) == 1 << 2) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(0, curTileSize / 2f, 0));
			vertList.Add(new Vector3(curTileSize / 2f, curTileSize / 2f, 0));
		}

		if ((bitmask & 1 << 3) == 1 << 3) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(curTileSize / 2f, curTileSize / 2f, 0));
			vertList.Add(new Vector3(curTileSize / 2f, 0, 0));
		}

		if ((bitmask & 1 << 4) == 1 << 4) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(curTileSize / 2f, 0, 0));
			vertList.Add(new Vector3(curTileSize / 2f, -curTileSize / 2f, 0));
		}

		if ((bitmask & 1 << 5) == 1 << 5) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(curTileSize / 2f, -curTileSize / 2f, 0));
			vertList.Add(new Vector3(0, -curTileSize / 2f, 0));
		}

		if ((bitmask & 1 << 6) == 1 << 6) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(0, -curTileSize / 2f, 0));
			vertList.Add(new Vector3(-curTileSize / 2f, -curTileSize / 2f, 0));
		}

		if ((bitmask & 1 << 7) == 1 << 7) {
			vertList.Add(Vector3.zero);
			vertList.Add(new Vector3(-curTileSize / 2f, -curTileSize / 2f, 0));
			vertList.Add(new Vector3(-curTileSize / 2f, 0, 0));
		}
		
		SetMeshVerts(vertList.ToArray());
	}

	void SetMeshVerts(Vector3[] verts) {
		meshFilter.mesh = null;

		mesh = new Mesh();
		meshFilter.mesh = mesh;
		
		Vector2[] uvs = new Vector2[verts.Length];
		int[] tris = new int[verts.Length];

		for (int i = 0; i < uvs.Length; i++) uvs[i] = Vector2.zero;
		for (int i = 0; i < tris.Length; i++) tris[i] = i;
		
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;

		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
}
