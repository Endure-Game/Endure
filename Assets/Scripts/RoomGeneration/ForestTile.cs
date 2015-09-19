using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ForestTile : BiomeTile
{
	// randomization constants
//	public int bloomNum = 100;
//	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);

	public const int BiomeNumber = 0;

	public override void RandomBlocking(List<Tile> region) {

		base.RandomBlocking(region);

		this.PerlinGenerator(region,
		                     new TilePlacer(this.placeBlockingTile),
		                     .47f,
		                     .2f);

		// place random item in the trees
		Tile centerTile = this.GetTreeSurrounded(region);
		if (centerTile != null) {
			Destroy (centerTile.item);
			this.GetComponent<RoomManager>().PlaceItem(this.GetComponent<ElevationTile>().randomItem,
																								 centerTile.x,
																								 centerTile.y);
			//print ("x:" + centerTile.x + "   y:" + centerTile.y);

			Tile lowerTile = this.tileMap[centerTile.x, centerTile.y - 1];
			Destroy (lowerTile.item);
		}

	}

	public Tile GetTreeSurrounded(List<Tile> tiles) {
		int biome = tiles[0].biome;
		foreach (Tile tile in tiles) {

			bool surrounded = true;
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {

					int xPos = tile.x + x;
					int yPos = tile.y + y;

					if (xPos < 0 || yPos < 0 || xPos >= width || yPos >= height) {
						surrounded = false;
					} else {
						Tile tilecheck = this.tileMap[xPos, yPos];
						if (biome != tilecheck.biome || tilecheck.item == null || tilecheck.item.GetComponent<HandleAxe>() == null) {
							surrounded = false;
						}
					}


				}
			}

			if (surrounded && tile.y >= 1) {
				return tile;
			}
		}

		return null;
	}

	public override int getBiomeNumber() {
		return 0;
	}
}
