using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tile : MonoBehaviour {
	public int bitmask {get; private set;}
	public TileHolder tileHolder {get; private set;}
	public bool hasBeenTouched {get; private set;}
	public Piece[] pieces;
	public float maxTapLength = 0.2f;

	public GameObject piecePrefab;
	public GameObject pieceHolder;
	public LevelManager levelManager;

	private bool isAnimating = false;
	private List<Tile> curOverlappingTiles;
	private Vector3 initialTouchPos;
	private float initialTouchTime = 0;
	private const int squareBitmask = 255;

	void Update () {
		if (Input.GetMouseButtonDown(0)) HandleMouseButtonDown();
		else if (Input.GetMouseButton(0)) HandleMouseButtonHold();
		else if (Input.GetMouseButtonUp(0)) HandleMouseButtonUp();
	}

	void HandleDroppedOnTile(Tile otherTile) {
		CombineBitmaskWithTile(otherTile);
		SetSemiRandomBitmask();
		//SetBitmask(0);
		ReturnToZeroPosition();
		SetPieceAlpha(0);
		SetPieceAlpha(1, 0.2f);
	}
	
	#region Game Logic
	void CompletedSquare(Tile completedSquareTile) {
		levelManager.CompletedSquare(completedSquareTile);
		completedSquareTile.SetSemiRandomBitmask();
	}
	#endregion

	#region Initialization
	void Start () {
		bitmask = 0;
		hasBeenTouched = false;
		levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		curOverlappingTiles = new List<Tile>();
		
		InitPieces();
		SetFullyRandomBitmask();
	}
	
	void InitPieces() {
		pieces = new Piece[8];
		
		for (int i = 0; i < 8; i++) {
			Piece newPiece = ((GameObject)Instantiate(piecePrefab, Vector3.zero, Quaternion.identity)).GetComponent<Piece>();
			newPiece.SetPieceType((PieceType)i);
			newPiece.transform.parent = pieceHolder.transform;
			newPiece.transform.localPosition = Vector3.zero;
			newPiece.GetComponentInChildren<MeshRenderer>().enabled = false;
			pieces[i] = newPiece;
		}
	}
	#endregion

	#region Input
	void HandleMouseButtonDown() {
		if (!isAnimating) {
			Ray ray = GameCamera.gameCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if(Physics.Raycast(ray, out hit)) {
				if (hit.transform == transform) {
					initialTouchPos = hit.point;
					initialTouchPos.z = 0;
					initialTouchTime = Time.time;
					hasBeenTouched = true;
				}
			}
		}
	}

	void HandleMouseButtonHold() {
		if (hasBeenTouched) UpdateWithMousePosition();
	}

	void HandleMouseButtonUp() {
		if (hasBeenTouched) {
			bool hasHeldLongerThanTap = Time.time - initialTouchTime > maxTapLength;

			Tile closestTile = GetClosestOverlappingTile();

			if (closestTile != null) HandleDroppedOnTile(closestTile);
			else {
				if (hasHeldLongerThanTap) ReturnToZeroPosition(0.1f);
				else HandleTapAndRelease();
			}

			hasBeenTouched = false;
		}
	}

	void HandleTapAndRelease() {
		ReturnToZeroPosition(0.02f);
		RotateTileBy45();
	}
	#endregion

	#region Unity Physics
	void OnTriggerEnter(Collider coll) {
		Tile tile = coll.GetComponent<Tile>();
		if (tile != null) curOverlappingTiles.Add(tile);
	}

	void OnTriggerExit(Collider coll) {
		Tile tile = coll.GetComponent<Tile>();
		if (tile != null) curOverlappingTiles.Remove(tile);
	}
	#endregion

	#region Positioning and Rotation
	public void ReturnToZeroPosition(float time = 0) {
		if (time == 0) transform.localPosition = Vector3.zero;
		else {
			isAnimating = true;
			Go.to(transform, time, new GoTweenConfig().localPosition(Vector3.zero).onComplete((tween) => {
				isAnimating = false;
			}));
		}
	}

	void UpdateWithMousePosition() {
		Vector3 curTouchPos = GameCamera.gameCam.ScreenToWorldPoint(Input.mousePosition);
		curTouchPos.z = 0;
		transform.localPosition = curTouchPos - initialTouchPos;
	}

	void RotateTileBy45() {
		isAnimating = true;
		//RotateBitmaskBy45();
		Go.to(transform, 0.05f, new GoTweenConfig().localEulerAngles(new Vector3(0, 0, -45), true).onComplete((tween) => {
			RotateBitmaskBy45();

			isAnimating = false;
		}));
	}

	void RotateBitmaskBy45() {
		Vector3 localEuler = transform.localEulerAngles;
		localEuler.z = 0;
		int newBitmask = 0;
		for (int i = 0; i < 8; i++) {
			bool oldBitOn = (bitmask & (1<<i)) == 1<<i;
			if (oldBitOn) newBitmask |= 1<<((i+1)%8);
		}
		SetBitmask(newBitmask);
		transform.localEulerAngles = localEuler;
	}
		    
	#endregion

	#region Helpers
	Tile GetClosestOverlappingTile() {
		float minDist = float.MaxValue;
		Tile closestTile = null;

		foreach (Tile t in curOverlappingTiles) {
			float dist = (t.transform.position - transform.position).magnitude;
			if (dist < minDist) {
				minDist = dist;
				closestTile = t;
			}
		}

		return closestTile;
	}
	
	public void SetTileHolder(TileHolder th) {
		Vector3 origPos = transform.position;
		transform.parent = th.transform;
		transform.position = origPos;
		th.tile = this;
		tileHolder = th;
	}
	#endregion

	#region Sprites
	void SetPieceAlpha(float alpha, float time = 0) {
		Color newColor = Color.white;
		newColor.a = alpha;

		foreach (Piece p in pieces) {
			if (time == 0) p.sprite.color = newColor;
			else Go.to(p.sprite, 0.3f, new GoTweenConfig().colorProp("color", newColor));
		}
	}
	#endregion
	
	#region Bitmasking
	public void SetBitmask(int bit) {
		bitmask = bit;
		
		for (int i = 0; i < 8; i++) {
			MeshRenderer pieceMeshRenderer = pieces[i].GetComponentInChildren<MeshRenderer>();
			pieceMeshRenderer.enabled = (bitmask & (1<<i)) == 1<<i;
		}
	}

	int GetInvertedBitmask(int bitmaskToInvert) {
		return ~bitmaskToInvert & Convert.ToInt32("11111111", 2);
	}

	// set this tile to the approximate opposite of another tile and return it to it's home position
	void SetBitmaskSimilarToRandomTile(float randomPercent) {
		int indexOfTileToCopy;
		
		do indexOfTileToCopy = UnityEngine.Random.Range(0, levelManager.numTiles);
		while (indexOfTileToCopy == tileHolder.index);
		
		int newBitmask = GetInvertedBitmask(levelManager.tileHolders[indexOfTileToCopy].tile.bitmask);
		int diffs = 0;
		for (int i = 0; i < 8; i++) {
			if (UnityEngine.Random.value < randomPercent) {
				diffs++;
				newBitmask ^= 1<<i;
			}
		}

		//Debug.Log("set tile " + tileHolder.index + " similar to tile " + indexOfTileToCopy + " with " + diffs + " diffs");

		SetBitmask(newBitmask);
	}

	public void SetSemiRandomBitmask() {
		if (UnityEngine.Random.value < 0.5f) SetBitmaskSimilarToRandomTile(0.2f);
		else SetBitmaskAsComplementOfTwoOtherTiles();
	}

	// make it so that this tile combined with another will be the perfect inversion of a third tile
	void SetBitmaskAsComplementOfTwoOtherTiles() {
		int indexOfGoalTile;
		int indexOfComplementTile;

		do indexOfGoalTile = UnityEngine.Random.Range(0, levelManager.numTiles);
		while (indexOfGoalTile == tileHolder.index);

		do indexOfComplementTile = UnityEngine.Random.Range(0, levelManager.numTiles);
		while (indexOfComplementTile == tileHolder.index || indexOfComplementTile == indexOfGoalTile);

		Tile goalTile = levelManager.tileHolders[indexOfGoalTile].tile;
		Tile complementTile = levelManager.tileHolders[indexOfComplementTile].tile;

		int goalTileAndComplementTileXOr = goalTile.bitmask ^ complementTile.bitmask;

		int newBitmask = GetInvertedBitmask(goalTileAndComplementTileXOr);
		
		//Debug.Log("new tile: " + tileHolder.index + ", goal tile: " + indexOfGoalTile + ", complement tile: " + indexOfComplementTile);
		
		SetBitmask(newBitmask);
	}

	void CombineBitmaskWithTile(Tile otherTile) {
		otherTile.SetBitmask(otherTile.bitmask ^ bitmask);
		//Debug.Log(Convert.ToString(otherTile.bitmask, 2));

		if ((otherTile.bitmask & squareBitmask) == squareBitmask) CompletedSquare(otherTile);
	}

	void SetFullyRandomBitmask() {
		int[] randomizedIndices = new int[] {0, 1, 2, 3, 4, 5, 6, 7};
		Array.Sort(randomizedIndices, delegate(int i1, int i2) { 
			return UnityEngine.Random.value < 0.5f ? 1 : -1;
		});
		
		int totalNumPieces = UnityEngine.Random.Range(2, 5);
		int newBitMask = 0;
		
		for (int i = 0; i < totalNumPieces; i++) {
			newBitMask |= 1<<randomizedIndices[i];
		}
		SetBitmask(newBitMask);
	}
	#endregion
}
