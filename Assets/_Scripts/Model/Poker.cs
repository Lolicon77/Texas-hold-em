using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class Poker : MonoBehaviour {

		public enum Suit {
			Spade,
			Heart,
			Club,
			Diamond,
		}

		public Suit suit;
		public int value;

	}
}