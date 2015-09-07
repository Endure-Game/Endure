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

		this.PerlinGenerator(region, 
		                     new TilePlacer(this.placeBlockingTile),
		                     .47f,
		                     .2f);
	}

	public override int getBiomeNumber() {
		return 0;
	}
}
