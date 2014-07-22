using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
	public GameObject tilePrefab;
	public GameObject tileHolderPrefab;
	public int score = 0;
	public int tileMargin = 50;
	public int puzzleWidth = 3;
	public int puzzleHeight = 3;
	public TileHolder[] tileHolders;
	public Tile[] tiles;

	public int numTiles {get; private set;}

	// Use this for initialization
	void Start () {
		numTiles = puzzleWidth * puzzleHeight;

		SetupTileHolders();
		SetupTiles();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetupTileHolders() {
		tileHolders = new TileHolder[numTiles];

		for (int i = numTiles - 1; i >= 0; i--) {
			TileHolder th = ((GameObject)Instantiate(tileHolderPrefab)).GetComponent<TileHolder>();
			th.name = "Tile Holder " + i.ToString();
			th.index = i;
			tileHolders[i] = th;
		}

		float puzzleWidthInPoints = puzzleWidth * tileMargin;
		float puzzleHeightInPoints = puzzleHeight * tileMargin;

		Vector2 origin = new Vector2(-puzzleWidthInPoints / 2f, puzzleHeightInPoints / 2f);

		for (int y = 0; y < puzzleHeight; y++) {
			for (int x = 0; x < puzzleWidth; x++) {
				tileHolders[x + y * puzzleWidth].transform.position = new Vector3(origin.x + (x + 0.5f) * tileMargin, origin.y - (y + 0.5f) * tileMargin, 0);
			}
		}
	}

	void SetupTiles() {
		tiles = new Tile[numTiles];

		for (int i = 0; i < numTiles; i++) {
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
