using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Model
{
	public class ValueCount
	{
		public int value;
		public int count;
	}

	public class FivePokerGroup
	{
		public static readonly string[] POKER_SHAPE_STRS = { "None", "高牌", "一对", "两对", "三张", "顺子", "同花", "葫芦", "金刚", "同花顺", };
		private const string FOUR_SPACE = "    ";

		public enum PokerShape
		{
			None,
			HighHand,
			OnePair,
			TwoPairs,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAKind,
			StraightFlush,
			//			RoyalFlush,
		}

		public List<Poker> pokers;
		public PokerShape shape;
		public List<ValueCount> valueCounts = new List<ValueCount>();

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (var poker in pokers)
			{
				stringBuilder.Append(poker).Append(FOUR_SPACE);
			}
			stringBuilder.Append(POKER_SHAPE_STRS[(int)shape]);
			return stringBuilder.ToString();
		}
	}
}