using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class TileHelper : MonoBehaviour {
	
	public GameObject[] prefabs;
	[Range(-100, 100)] public float probabilitySlope, bIntercept = 0;
	
	public void spawnEntities(int x) {
		float _x = x;
		spawnEntities(_x);
	}
	
	public void spawnEntities(float x) {
		if (prefabs.Length <= 0)
			return;
		
		float rand = Random.Range(0f, 100f);
		float result = probabilitySlope * x + bIntercept;
		
		if (rand < result) {
			int _rand = Random.Range(0, prefabs.Length);
			Instantiate(prefabs[_rand], transform.position, Quaternion.identity, transform);
		}
	}
	
	public override string ToString () {    //Return a string representing the class
		if (transform.childCount > 0) {
			return string.Format ("***Class TileHelper*** Name: {0} | Spawned Child: {1} ",
				this.gameObject.name, transform.GetChild(0).gameObject.name);
		}
		else return string.Format ("***Class TileHelper*** Name: {0} ",
				this.gameObject.name);
				
	}
}
}