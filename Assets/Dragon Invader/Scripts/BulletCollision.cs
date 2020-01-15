using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class BulletCollision	 : MonoBehaviour {
	
	public LayerMask players;
	public LayerMask enemies;
	public LayerMask terrain;
	
	private void OnTriggerEnter2D(Collider2D c)
	{
		#region Future Reference i guess
		/*
		Debug.Log(this.gameObject.tag + " wtf" + c.gameObject.layer + " id kdawd awd " + c.gameObject.tag +  " WAAJW(*JAWJAWIT " + players + " WAFIAWFIOAWJF " + enemies);
		Debug.Log(players.value & 1 << c.gameObject.layer);				//	0
		Debug.Log(1 << c.gameObject.layer);								//	pow(10)
		Debug.Log(players.value);										//	pow(9)
		Debug.Log(1 << c.gameObject.layer & 1 << c.gameObject.layer);	//	pow(10)
		Debug.Log(c.gameObject.layer);									//	10
		Debug.Log(1 << terrain.value);									//	1
		Debug.Log((1 << terrain.value) & (1 << terrain.value));			//	1
		Debug.Log((1 << terrain.value) & (1 << c.gameObject.layer));	//	0
		Debug.Log((1 << terrain.value) == (1 << c.gameObject.layer));	//	false
		Debug.Log((1 << terrain.value) & players.value);				//	0
		Debug.Log((1 << terrain.value) & terrain.value);				//	0
		Debug.Log(terrain.value & players.value);						//	pow(9)
		Debug.Log(terrain.value & (1 << players.value) & c.gameObject.layer);	// 0
		Debug.Log((terrain.value & (1 << c.gameObject.layer)) != 0);		// true
		Debug.Log((enemies.value & (1 << c.gameObject.layer)) != 0);		// true
		Debug.Log((players.value & (1 << c.gameObject.layer)) != 0);		// false
		*/
		#endregion
		
		if (((players.value & (1 << c.gameObject.layer)) != 0) && this.gameObject.tag == "Enemy") {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log("This bullet, " + gameObject.name + ", has touched a player: " + c.gameObject.name);
			#endif
			
			c.GetComponent<Character>().ModifyHealth(-1);
			Destroy(this.gameObject);
		} else if (((enemies.value & (1 << c.gameObject.layer)) != 0) && this.gameObject.tag == "Player") {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log("This bullet, " + gameObject.name + ", has touched a player: " + c.gameObject.name);
			#endif
			
			c.GetComponent<Character>().ModifyHealth(-1);
			Destroy(this.gameObject);
		} else if ((terrain.value & (1 << c.gameObject.layer)) != 0) {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log("This bullet, " + gameObject.name + ", has collided into a wall.");
			#endif
			
			Destroy(this.gameObject);
		} else {
			#if SHOW_DEBUG_MESSAGES
			Debug.Log("Bullet missed i guess.");
			#endif
		}
	}
}
}
