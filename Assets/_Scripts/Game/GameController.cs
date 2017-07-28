using System.Collections.Generic;
using L7;
using UnityEngine;

namespace Game {
	public class GameController : SingletonForMonoInstantiateOnAwake<GameController> {

		public List<Poker> pokerList;
		public List<Poker> pokersOnTable;

		public void Deal(int num, out List<Poker> outPokers) {
			outPokers = new List<Poker>();
			for (int i = pokerList.Count - 1; i > 0; i--) {
				outPokers.Add(outPokers[i]);
				pokerList.RemoveAt(i);
			}
		}

		//		public bool Compare(List<Poker> a, List<Poker> b) {
		//		}

	}
}