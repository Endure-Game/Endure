using UnityEngine;
using System.Collections.Generic;

public class Tile
{
	public int biome;
	public bool blocking;
	public int elevation;
	public GameObject item;

	public Tile (int biome, bool blocking, int elevation) {
		this.biome = biome;
		this.blocking = blocking;
		this.elevation = elevation;
	}

	public Tile (Tile tile) {
		this.biome = tile.biome;
		this.blocking = tile.blocking;
		this.elevation = tile.elevation;
	}
}

