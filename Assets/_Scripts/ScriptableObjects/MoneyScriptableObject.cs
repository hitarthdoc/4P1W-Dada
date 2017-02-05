using UnityEngine;
using System.Collections;

namespace SO.Money
{

	public class MoneyScriptableObject : ScriptableObject
	{

		[SerializeField]
		private int moneyEarned = 100;

		[SerializeField]
		private int moneyEarnedPerLevel;

		[SerializeField]
		private int moneySpentOnPowerUp_1;

		[SerializeField]
		private int moneySpentOnPowerUp_2;

		public int MoneyEarned {
			get
			{
				return moneyEarned;
			}
		}

		public void IncreaseMoney (int increaseBy)
		{
			moneyEarned += increaseBy;
		}

		public void IncreaseMoneyOnLevelCleared ()
		{
			moneyEarned += moneyEarnedPerLevel;
		}

		public void DecreaseMoneyOnPowerUp_1 ()
		{
			moneyEarned -= moneySpentOnPowerUp_1;
		}

		public void DecreaseMoneyOnPowerUp_2 ()
		{
			moneyEarned -= moneySpentOnPowerUp_2;
		}

	}
}
