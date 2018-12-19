using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class GamePool {

		public struct PlayerWithMoney {
			public Player player;
			public int moneyThisTurn;
			public int allmoney;
		}

		public List<PlayerWithMoney> playerWithMonies;

	}
}