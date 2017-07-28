using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {
	public class Checker : MonoBehaviour {
		public enum PokerShape {
			None,
			OnePair,
			TwoPairs,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAkind,
			StraightFlush,
			//			RoyalFlush,
		}

		int CheckStraight(List<Poker> pokers, List<int> values, out bool straightFlush) {
			pokers.Sort((a, b) => {
				if (a.value == b.value) {
					return 0;
				}
				return a.value < b.value ? 1 : -1;
			});
			straightFlush = false;
			for (int i = 0; i < 3; i++) {
				var value = pokers[i].value;
				var straight = true;
				for (int j = value - 1; j > value - 5; j--) {
					if (j <= 0) {
						straight = false;
						break;
					}
					if (!values.Contains(j)) {
						straight = false;
						break;
					}
				}
				if (straight) {
					var tempSuit = pokers[i].suit;
					straightFlush = true;
					for (int j = i + 1; j < i + 5; j++) {
						if (pokers[j].suit != tempSuit) {
							straightFlush = false;
							break;
						}
					}
					return value;
				}
			}
			return -1;
		}

		public PokerShape CalculatePokerShape(List<Poker> pokers) {
			Dictionary<Poker.Suit, int> suitCountDic = new Dictionary<Poker.Suit, int>();
			Dictionary<int, int> valueCountDic = new Dictionary<int, int>();
			List<int> valueList = new List<int>();
			bool straight = false;
			bool flush = false;
			Poker.Suit flushSuit;

			int FourOfAKindValue;

			for (int i = 0; i < pokers.Count; i++) {
				var poker = pokers[i];
				if (!suitCountDic.ContainsKey(poker.suit)) {
					suitCountDic.Add(poker.suit, 1);
				} else {
					suitCountDic[poker.suit] += 1;
				}
				valueList.Add(poker.value);
				//				if (!valueCountDic.ContainsKey(poker.value)) {
				//					valueCountDic.Add(poker.value, 1);
				//				} else {
				//					valueCountDic[poker.value] += 1;
				//				}
			}


			foreach (var suitCount in suitCountDic) {
				if (suitCount.Value >= 5) {
					flush = true;
					flushSuit = suitCount.Key;
				}
			}

			foreach (var valueCount in valueCountDic) {
			}

			return PokerShape.None;

		}

	}
}