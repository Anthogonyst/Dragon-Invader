using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class Molerat : Character {
	
	SpriteRenderer sprite;
	Color _color;

	protected override IEnumerator Walk() {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		float frames = 30;
		float dir = Random.Range(0, 360);
		
		#if SHOW_DEBUG_MESSAGES
		Debug.Log(gameObject.name + " is burrowing.");
		#endif
		
		if (sprite == null) {
			sprite = GetComponent<SpriteRenderer>();
			_color = sprite.color;
		}
		
		if (sprite != null) {
			_color.a = 0;
			sprite.color = _color;
		}
		
		IEnumerator corout = Interrupt(new string[] { "Walk" }, frames);
		StartCoroutine(corout);
		
		while (frames > 0) {
			Vector3 mov = new Vector3(speed * Mathf.Cos(dir) * Time.deltaTime, speed * Mathf.Sin(dir) * Time.deltaTime, 0);
			transform.position += mov;
		
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		if (sprite != null) {
			_color.a = 1;
			sprite.color = _color;
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Walk");
		Coinflip(activity.Movement);
	}
}
}