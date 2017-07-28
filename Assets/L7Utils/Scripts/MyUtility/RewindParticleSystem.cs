#if UNITY_5_5_OR_NEWER
using UnityEngine;
using System.Collections;

namespace L7 {
	public class RewindParticleSystem : MonoBehaviour {

		public new ParticleSystem particleSystem;

		private float duration;

		private float timer;
		private float elapsedTime;

		void OnValidate() {
			if (particleSystem == null) {
				particleSystem = GetComponent<ParticleSystem>();
			}
		}

		void Start() {
			particleSystem.Pause(true);
			duration = particleSystem.main.duration;
			timer = Time.time;
		}


		void Update() {
			elapsedTime = Time.time - timer;
			if (elapsedTime < duration) {
				//				particleSystem.Clear();
				particleSystem.Simulate(duration - elapsedTime, true);
				//				particleSystem.Play();
			} else {
				Destroy(this);
			}
		}

	}
}
#endif