using System.Collections;
using UnityEngine;

public class DestroyAfterTimeParticle : MonoBehaviour {
	[Tooltip("Time to destroy")]
	public float timeToDestroy = 0.8f;
	/*
	* Destroys gameobject after its created on scene.
	* This is used for particles and flashes.
	*/
	void Start () {
		if (gameObject.tag != "BulletHole")
			Destroy (gameObject, timeToDestroy);
	}

}
