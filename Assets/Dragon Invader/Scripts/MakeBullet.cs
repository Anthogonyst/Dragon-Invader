using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBullet : MonoBehaviour {

	public Transform projectile, clone;

	void Update() {
		if (Input.GetButtonDown("Fire1")) {
			// Instantiate the projectile at the position and rotation of this transform
			clone = Instantiate(projectile, transform.position, transform.rotation);
		}
	}
}
