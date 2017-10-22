﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	//The Grid itself
	public static int w = 10;
	public static int h = 25;
	public static Transform[,] grid = new Transform[w, h];
	private const int POINTS_PER_ROW = 100;

	public static Vector2 roundVec2(Vector2 v) {
		return new Vector2(Mathf.Round(v.x),
			Mathf.Round(v.y));
	}

	public static bool insideBorder(Vector2 pos) {
		return ((int)pos.x >= 0 &&
			(int)pos.x < w &&
			(int)pos.y >= 0);
	}


	public static void deleteRow(int y) {
		for (int x = 0; x < w; ++x) {
			var _block = grid [x, y].gameObject;
			grid [x, y] = null;
			var _blockChild = _block.transform.GetChild(0).gameObject;
			_blockChild.transform.SetParent (null, true);
			Destroy (_block);
			Block _blockScript = _blockChild.GetComponent<Block> ();
			_blockScript.drain ();
		}
		Points.puntos += POINTS_PER_ROW;

	}

	public static void decreaseRow(int y) {
		for (int x = 0; x < w; ++x) {
			if (grid[x, y] != null) {
				// Move one towards bottom
				grid[x, y-1] = grid[x, y];
				grid[x, y] = null;

				// Update Block position
				grid[x, y-1].position += new Vector3(0, -1, 0);
			}
		}

	}

	public static void decreaseRowsAbove(int y) {
		for (int i = y; i < h; ++i)
			decreaseRow(i);
	}

	public static bool isRowFull(int y) {
		for (int x = 0; x < w; ++x)
			if (grid[x, y] == null)
				return false;
		return true;
	}

	public static void deleteFullRows() {
		bool _atLeastOneFullRow = false;
		for (int y = 0; y < h; ++y) {
			if (isRowFull(y)) {
				deleteRow(y);
				decreaseRowsAbove(y+1);
				--y;

				_atLeastOneFullRow = true;
			}
		}

		SoundPlayer.Instance.playSlurp ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
