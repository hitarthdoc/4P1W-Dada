using UnityEngine;
using System.Collections;

namespace SO.Money
{

	public class MoneyScriptableObject : ScriptableObject
	{

		#if UNITY_EDITOR

		const int defaultMoneyEarned = 100;

		const int defaultMoneyEarnedPerLevel = 4;

		const int defaultMoneySpentOnPowerUp_1 = 10;

		const int defaultMoneySpentOnPowerUp_2 = 20;

		public void ResetMoneySO ()
		{
			moneyEarned = defaultMoneyEarned;
			moneyEarnedPerLevel = defaultMoneyEarnedPerLevel;
			moneySpentOnPowerUp_1 = defaultMoneySpentOnPowerUp_1;
			moneySpentOnPowerUp_2 = defaultMoneySpentOnPowerUp_2;
		}

		public void AddMoney ( int IncreaseBy )
		{
			IncreaseMoney ( IncreaseBy );
		}

		public void DecreaseMoney ( int DecreaseBy )
		{
			IncreaseMoney ( -DecreaseBy );
		}

		public void ResetMonetEarned ()
		{
			moneyEarned = defaultMoneyEarned;
		}

		public int MoneyEarnedPerLevel {
			get
			{
				return moneyEarnedPerLevel;
			}
			set
			{
				moneyEarnedPerLevel = value;
			}
		}

		public int MoneySpentOnPowerUp_1 {
			get
			{
				return moneySpentOnPowerUp_1;
			}
			set
			{
				moneySpentOnPowerUp_1 = value;
			}
		}

		public int MoneySpentOnPowerUp_2 {
			get
			{
				return moneySpentOnPowerUp_2;
			}
			set
			{
				moneySpentOnPowerUp_2 = value;
			}
		}

		#endif

		[SerializeField]
		private int moneyEarned = 100;

		[SerializeField]
		private int moneyEarnedPerLevel = 4;

		[SerializeField]
		private int moneySpentOnPowerUp_1 = 10;

		[SerializeField]
		private int moneySpentOnPowerUp_2 = 20;

		public int MoneyEarned {
			get
			{
				return moneyEarned;
			}
			#if UNITY_EDITOR
			set
			{
				moneyEarned = value;
			}
			#endif
		}

		void OnEnable ()
		{
			if ( PlayerPrefs.HasKey ( "moneyEarned" ) )
			{
				moneyEarned = PlayerPrefs.GetInt ( "moneyEarned" );
			}
			else
			{
				PlayerPrefs.SetInt ( "moneyEarned", moneyEarned );
			}
			PlayerPrefs.Save ();

		}

		public void IncreaseMoney ( int increaseBy )
		{
			moneyEarned += increaseBy;
			PlayerPrefs.SetInt ( "moneyEarned", moneyEarned );
			PlayerPrefs.Save ();
		}

		public void IncreaseMoneyOnLevelCleared ()
		{
			moneyEarned += moneyEarnedPerLevel;
			PlayerPrefs.SetInt ( "moneyEarned", moneyEarned );
			PlayerPrefs.Save ();
		}

		public bool DecreaseMoneyOnPowerUp_1 ()
		{
			if ( ( moneyEarned - moneySpentOnPowerUp_1 ) >= 0 )
			{
				moneyEarned -= moneySpentOnPowerUp_1;
				PlayerPrefs.SetInt ( "moneyEarned", moneyEarned );
				PlayerPrefs.Save ();
				return true;
			}
			return false;
		}

		public bool DecreaseMoneyOnPowerUp_2 ()
		{
			if ( ( moneyEarned - moneySpentOnPowerUp_2 ) >= 0 )
			{
				moneyEarned -= moneySpentOnPowerUp_2;
				PlayerPrefs.SetInt ( "moneyEarned", moneyEarned );
				PlayerPrefs.Save ();
				return true;
			}
			return false;
		}

		public void RefundMoneyOnPowerUp_2 ()
		{
			moneyEarned += moneySpentOnPowerUp_2;
			PlayerPrefs.SetInt ( "moneyEarned", moneyEarned );
			PlayerPrefs.Save ();
		}

	}
}
