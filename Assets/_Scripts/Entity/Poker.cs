using UnityEngine;
using UnityEngine.UI;

namespace Model
{
	public class Poker
	{
		public enum Suit
		{
			Spade,
			Heart,
			Club,
			Diamond,
		}

		public Suit suit;
		public int value;

		private static string[] suits = { "黑桃", "红桃", "梅花", "方块" };
		private static string[] values = { "J", "Q", "K", "A" };

		public override string ToString()
		{
			return suits[(int)suit] + (value <= 10 ? value.ToString() : values[value - 11]);
		}
	}
}