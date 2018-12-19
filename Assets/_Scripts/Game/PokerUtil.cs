using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L7;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
	public class PokerUtil : SingletonForMonoInstantiateOnAwake<PokerUtil>
	{
		private List<Poker> _currentSevenPokers = new List<Poker>();
		private int[] _currentGroupValues = new int[5];
		private Poker.Suit[] _currentGroupSuits = new Poker.Suit[5];

		private List<int[]> _allIndexes = new List<int[]>();
		private FivePokerGroup _currentMaxGroup;
		private FivePokerGroup _currentGroup;

		private List<Poker> AllPokers = new List<Poker>();

		public int testCount = 1;

		void Start()
		{
			for (int i = 0; i < 4; i++)
			{
				var suit = (Poker.Suit)i;
				for (int j = 0; j < 13; j++)
				{
					var value = j + 2;
					AllPokers.Add(new Poker() { suit = suit, value = value });
				}
			}
			for (int i = 0; i <= 9; i++)
			{
				countDic.Add((FivePokerGroup.PokerShape)i, 0);
			}
			GetCombination(_allIndexes, null, 7, 5, 0);
			StopWatchUtil.QuickStart(Test2, testCount);
			foreach (var kv in countDic)
			{
				L7Debug.Log(FivePokerGroup.POKER_SHAPE_STRS[(int)kv.Key] + "   " + (float)kv.Value / testIndexes.Count);
			}
		}

		private StringBuilder stringBuilder = new StringBuilder();
		private Dictionary<FivePokerGroup.PokerShape, int> countDic = new Dictionary<FivePokerGroup.PokerShape, int>();
		private List<int[]> testIndexes = new List<int[]>();


		void Test()
		{
			//			for (int i = 0; i < 3; i++)
			//			{
			//				AllPokers.Shuffle();
			//			}
			//			stringBuilder.Length = 0;
			testIndexes = new List<int[]>();
			GetCombination(testIndexes, null, 52, 7, 0);
			foreach (var testIndex in testIndexes)
			{
				_currentSevenPokers.Clear();
				for (int i = 0; i < 7; i++)
				{
					_currentSevenPokers.Add(AllPokers[testIndex[i]]);
					//				stringBuilder.Append(AllPokers[i] + "   ");
				}
				//			L7Debug.Log(stringBuilder.ToString());
				//			stringBuilder.Length = 0;
				FindMaximumGroup();
				countDic[_currentMaxGroup.shape]++;
				//			L7Debug.Log(_currentMaxGroup.ToString());
			}
		}

		void Test2()
		{
			testIndexes = new List<int[]>();
			GetCombination(testIndexes, null, 52, 5, 0);
			foreach (var testIndex in testIndexes)
			{
				FivePokerGroup group = new FivePokerGroup() { pokers = new List<Poker>() };
				for (int i = 0; i < 5; i++)
				{
					group.pokers.Add(AllPokers[testIndex[i]]);
				}
				CalculateShape(group);
				countDic[group.shape]++;
			}
		}

		public void SetPokers(List<Poker> pokers)
		{
			_currentSevenPokers = pokers;
		}

		private void GetCombination(List<int[]> allGroups, Stack<int> stack, int total, int n, int current)
		{
			if (stack == null)
			{
				stack = new Stack<int>();
			}
			if (n == 0)
			{
				var array = new int[n];
				foreach (var i in stack)
				{
					array = stack.ToArray().Reverse().ToArray();
				}
				allGroups.Add(array);
				return;
			}
			for (int i = current; i < total - n + 1; i++)
			{
				stack.Push(i);
				GetCombination(allGroups, stack, total, n - 1, i + 1);
				stack.Pop();
			}
		}

		private FivePokerGroup GetFivePokerGroupByIndex(int index)
		{
			FivePokerGroup group = new FivePokerGroup();
			group.pokers = new List<Poker>();
			foreach (var i in _allIndexes[index])
			{
				group.pokers.Add(_currentSevenPokers[i]);
			}
			return group;
		}

		public void FindMaximumGroup()
		{
			_currentMaxGroup = GetFivePokerGroupByIndex(0);
			for (int i = 1; i < _allIndexes.Count; i++)
			{
				_currentGroup = GetFivePokerGroupByIndex(i);
				if (ComparePoker(_currentMaxGroup, _currentGroup) == -1)
				{
					_currentMaxGroup = _currentGroup;
				}
			}
		}

		public int ComparePoker(FivePokerGroup a, FivePokerGroup b)
		{
			CalculateShape(a);
			CalculateShape(b);
			if (a.shape == b.shape)
			{
				return CompareGroupValue(a, b);
			}
			return a.shape.CompareTo(b.shape);
		}

		private int CompareGroupValue(FivePokerGroup a, FivePokerGroup b)
		{
			for (int i = a.valueCounts.Count - 1; i >= 0; i--)
			{
				var valueA = a.valueCounts[i].value;
				var valueB = b.valueCounts[i].value;
				if (valueA == valueB)
				{
					continue;
				}
				return valueA.CompareTo(valueB);
			}
			return 0;
		}

		private void CalculateShape(FivePokerGroup group)
		{
			if (group.shape != FivePokerGroup.PokerShape.None)
			{
				return;
			}

			CalculateValuesCounts(group);
			var flush = CheckFlush(group.pokers);
			var straight = CheckStraight(group.valueCounts);
			if (flush && straight)
			{
				group.shape = FivePokerGroup.PokerShape.StraightFlush;
			}
			else if (CheckFourOfAKind(group.valueCounts))
			{
				group.shape = FivePokerGroup.PokerShape.FourOfAKind;
			}
			else if (CheckFullHouse(group.valueCounts))
			{
				group.shape = FivePokerGroup.PokerShape.FullHouse;
			}
			else if (flush)
			{
				group.shape = FivePokerGroup.PokerShape.Flush;
			}
			else if (straight)
			{
				group.shape = FivePokerGroup.PokerShape.Straight;
			}
			else if (CheckThreeOfAKind(group.valueCounts))
			{
				group.shape = FivePokerGroup.PokerShape.ThreeOfAKind;
			}
			else if (CheckTwoPairs(group.valueCounts))
			{
				group.shape = FivePokerGroup.PokerShape.TwoPairs;
			}
			else if (CheckOnePair(group.valueCounts))
			{
				group.shape = FivePokerGroup.PokerShape.OnePair;
			}
			else
			{
				group.shape = FivePokerGroup.PokerShape.HighHand;
			}
		}

		private void CalculateValuesCounts(FivePokerGroup group)
		{
			foreach (var poker in group.pokers)
			{
				bool exist = false;
				foreach (var valueCount in group.valueCounts)
				{
					if (valueCount.value == poker.value)
					{
						valueCount.count++;
						exist = true;
						break;
					}
				}
				if (!exist)
				{
					group.valueCounts.Add(new ValueCount() { value = poker.value, count = 1 });
				}
			}
			group.valueCounts.Sort((a, b) => a.count == b.count ? -a.value.CompareTo(b.value) : -a.count.CompareTo(b.count));
		}


		private bool CheckThreeOfAKind(List<ValueCount> dic)
		{
			return dic[0].count == 3;
		}

		private bool CheckTwoPairs(List<ValueCount> dic)
		{
			return dic[0].count == 2 && dic[1].count == 2;
		}

		private bool CheckOnePair(List<ValueCount> dic)
		{
			return dic[0].count == 2;
		}

		bool CheckFlush(List<Poker> pokers)
		{
			for (var i = 0; i < pokers.Count - 1; i++)
			{
				if (pokers[i + 1].suit != pokers[i].suit)
				{
					return false;
				}
			}
			return true;
		}

		bool CheckStraight(List<ValueCount> dic)
		{
			if (dic.Count < 5)
			{
				return false;
			}
			if (dic[0].value == 14 && dic[1].value == 5)
			{
				return true;
			}
			return dic[0].value - dic[4].value == 4;
		}

		bool CheckFourOfAKind(List<ValueCount> dic)
		{
			return dic[0].count == 4;
		}

		bool CheckFullHouse(List<ValueCount> dic)
		{
			return dic.Count == 2 && dic[0].count == 3;
		}

	}
}