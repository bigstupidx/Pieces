using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject tilePrefab;
	public GameObject tileHolderPrefab;
	public int score = 0;
	public int tileMargin = 50;
	public TileHolder[] tileHolders;
	public Tile[] tiles;

	// Use this for initialization
	void Start () {
		SetupTileHolders();
		SetupTiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetupTileHolders() {
		tileHolders = new TileHolder[9];
		for (int i = 8; i >= 0; i--) {
			TileHolder th = ((GameObject)Instantiate(tileHolderPrefab)).GetComponent<TileHolder>();
			th.name = "Tile Holder " + i.ToString();
			th.index = i;
			tileHolders[i] = th;
		}

		tileHolders[0].transform.position = new Vector3(-tileMargin, tileMargin, 0);
		tileHolders[1].transform.position = new Vector3(0, tileMargin, 0);
		tileHolders[2].transform.position = new Vector3(tileMargin, tileMargin, 0);
		tileHolders[3].transform.position = new Vector3(-tileMargin, 0, 0);
		tileHolders[4].transform.position = new Vector3(0, 0, 0);
		tileHolders[5].transform.position = new Vector3(tileMargin, 0, 0);
		tileHolders[6].transform.position = new Vector3(-tileMargin, -tileMargin, 0);
		tileHolders[7].transform.position = new Vector3(0, -tileMargin, 0);
		tileHolders[8].transform.position = new Vector3(tileMargin, -tileMargin, 0);
	}

	void SetupTiles() {
		tiles = new Tile[9];

		for (int i = 0; i < 9; i++) {
			Tile tile = ((GameObject)Instantiate(tilePrefab)).GetComponent<Tile>();
			tile.transform.parent = tileHolders[i].transform;
			tile.SetTileHolder(tileHolders[i]);
			tile.ReturnToZeroPosition();
			tile.transform.localPosition = Vector3.zero;
			tiles[i] = tile;
		}
	}

	public void CompletedSquare(Tile completedSquareTile) {
		score++;
		Debug.Log(score);
	}
}
