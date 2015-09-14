using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElevationTile : MonoBehaviour
{
	public GameObject[] tiles;
	public GameObject xMarksTheSpot;
	public GameObject hole;
	public GameObject ropeLadder;

	public void SmoothElevation(List<Tile> tiles) {

		Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;
		int height = tileMap.GetLength(0);
		int width = tileMap.GetLength(1);

		bool smoothing = true;
		while(smoothing) {
			smoothing = false;
			foreach (Tile tile in tiles) {

				if (tile.x > 0 && tile.y > 0 && tile.x < width - 1 && tile.y < height - 1) {
					for (int xDelta = -1; xDelta <= 1; xDelta++) {
						for (int yDelta = -1; yDelta <= 1; yDelta++) {
							if (tileMap[tile.x + xDelta, tile.y + yDelta].elevation + 1 < tile.elevation) {
								tile.elevation--;
								smoothing = true;
							}
						}
					}
				}
			}
		}
	}

	public void placeCliffTiles(List<Tile> tiles) {

		Tile[,] tileMap = this.GetComponent<RoomManager>().tileMap;
		int height = tileMap.GetLength(0);
		int width = tileMap.GetLength(1);

		foreach (Tile tile in tiles) {
			int x = tile.x;
			int y = tile.y;

			if (x > 0 && y > 0 && x < width - 1 && y < height - 1) {
				bool lower = false;
				List<int> walls = new List<int>();
				for (int xDelta = -1; xDelta <= 1; xDelta++) {
					for (int yDelta = -1; yDelta <= 1; yDelta++) {

						if (tileMap[x + xDelta, y + yDelta].elevation > tileMap[x, y].elevation) {
							lower = true;
							walls.Add(xDelta + 1 + (yDelta + 1) * 3);
						}
					}
				}

				if (lower) {
					if (tile.path) {
						this.GetComponent<RoomManager>().SetGroundTile(this.tiles[0], x, y);
					} else {
						this.GetComponent<RoomManager>().PlaceItem(this.GetWallTile(walls), x, y);

						// Plains needs a special ground tile for layering
						if (tile.biome == this.GetComponent<PlainsTile>().getBiomeNumber()) {
							GameObject flatSprite = this.GetComponent<PlainsTile>().getFlatGroundTile();
							this.GetComponent<RoomManager>().SetGroundTile(flatSprite, x, y);
						}
					}
					tile.blocking = true;
				}
			}
		}
	}

	public GameObject GetTreasureTile() {
		return this.xMarksTheSpot;
	}

	// DONT TOUCH MY MAGIC FUNCTION --Chris
	private GameObject GetWallTile(List<int> walls) {

		if (walls.Contains(2)) {
			if (walls.Contains(1) && walls.Contains(5)) {
				return this.tiles[10];
			} else if (walls.Contains(1)) {
				if (walls.Contains(3)) {
					return this.tiles[11];
				} else {
					return this.tiles[1];
				}
			} else if (walls.Contains(5)) {
				if (walls.Contains(7)) {
					return this.tiles[9];
				} else {
					return this.tiles[7];
				}
			} else {
				return this.tiles[8];
			}
		} else if (walls.Contains(0)) {
			if (walls.Contains(1) && walls.Contains(3)) {
				return this.tiles[11];
			} else if (walls.Contains(1)) {
				return this.tiles[1];
			} else if (walls.Contains(3)) {
				if (walls.Contains(7)) {
					return this.tiles[12];
				} else {
					return this.tiles[3];
				}
			} else {
				return this.tiles[2];
			}
		} else if (walls.Contains(6)) {
			if (walls.Contains(7) && walls.Contains(3)) {
				return this.tiles[12];
			} else if (walls.Contains(7)) {
				return this.tiles[5];
			} else if (walls.Contains(3)) {
				return this.tiles[3];
			} else {
				return this.tiles[4];
			}
		} else if (walls.Contains(8)) {
			if (walls.Contains(7) && walls.Contains(5)) {
				return this.tiles[9];
			} else if (walls.Contains(7)) {
				return this.tiles[5];
			} else if (walls.Contains(5)) {
				return this.tiles[7];
			} else {
				return this.tiles[6];
			}
		}

		if (walls.IndexOf(1) != -1) {
			return this.tiles[1];
		} else if (walls.IndexOf(3) != -1) {
			return this.tiles[3];
		} else if (walls.IndexOf(5) != -1) {
			return this.tiles[7];
		} else if (walls.IndexOf(7) != -1) {
			return this.tiles[5];
		}
		print ("elevation placement probems --Show Chris your game if you see this");
		return this.tiles[0];
	}

	public GameObject GetXMarksTheSpot() {
		return this.xMarksTheSpot;
	}

	public GameObject GetHole() {
		return this.hole;
	}
}

