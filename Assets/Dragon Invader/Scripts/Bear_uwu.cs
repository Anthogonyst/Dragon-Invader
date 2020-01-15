using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragonInvader {
public class Bear_uwu : Character {
	
	protected override IEnumerator Walk() {
		bool _busy = false;
		bool _interrupt = true;
		checkStatus(_busy, _interrupt);
		
		Vector3 rotationalVector;
		float frames = 10;
		float dir = Random.Range(0, 360);
		
		#if SHOW_DEBUG_MESSAGES
		Debug.Log(gameObject.name + " is predding.");
		#endif
		
		while (frames > 0) {
			Vector3 mov = new Vector3(speed * Mathf.Cos(dir) * Time.deltaTime, speed * Mathf.Sin(dir) * Time.deltaTime, 0);
			if (enemyTarget != null && items[0] != null) {
				rotationalVector = transform.position - enemyTarget.transform.position;
				if (!(rotationalVector.magnitude > sightRange || Physics.Linecast(transform.position, enemyTarget.transform.position, bitmap, QueryTriggerInteraction.Ignore))) {
					Vector3 relative = transform.InverseTransformPoint(enemyTarget.transform.position);
					mov = 4f * speed * Vector3.Normalize(relative) * Time.deltaTime;
				}
			}
			
			transform.position += mov;
		
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Walk");
		Coinflip(activity.Movement);
	}
	
	protected override IEnumerator Attack() {
		bool _busy = true;
		bool _interrupt = false;
		checkStatus(_busy, _interrupt);
		
		float frames = 10;
		
		//if((mask.value & 1<<c.gameObject.layer) == 1<<c.gameObject.layer)
		#if SHOW_DEBUG_MESSAGES
		if (enemyTarget != null)
			Debug.Log(gameObject.name + " is attacking the " + enemyTarget.name + "!");
		else Debug.Log(gameObject.name + " is looking for something to attack.");
		#endif
		
		for (int i = 0; i < 4; i++) {
			while (frames > 0) {
				frames--;
				yield return new WaitForEndOfFrame();
			}
			makeAttackHitbox();
			frames = 10;
		}
		
		while (frames > 0) {
			frames--;
			yield return new WaitForEndOfFrame();
		}
		
		_busy = false;
		checkStatus(_busy, _interrupt);
		finishExistingState("Attack");
		Coinflip(activity.Action);
	}
}
}