using UnityEngine;
using VoidCEEC.Core;
using VoidCEEC.Shared;

namespace VoidCEEC.UCR.Manager
{
	public class GameManager : Singleton<GameManager>
	{
		[Header("CheckPoint")]
		[SerializeField] private GenericGameEventListener checkPointEvents;
		[SerializeField] private int numCheckPoints;

		[Header("Coin")]
		[SerializeField] public float numCoins = 1000;
		[SerializeField] public int currentCoin;
		[SerializeField] private GenericGameEventListener coinEvents;

		[Header( "TimeLab" )]
		[SerializeField] public int maxTime;

		[Header("Player")]
		[SerializeField] private GenericGameEventListener onTriggerOutLine;

		public int CurrentCheckpoint { get; private set; }

		private void OnEnable()
		{
			checkPointEvents.Subscribe();
			coinEvents.Subscribe();
			onTriggerOutLine.Subscribe();
		}

		private void OnDisable()
		{
			checkPointEvents.Unsubscribe();
			coinEvents.Unsubscribe();
			onTriggerOutLine.Unsubscribe();
		}

		private void Start()
		{
			CurrentCheckpoint = 0;
			currentCoin = 0;

			if ( checkPointEvents != null )
			{
				checkPointEvents.EventHandler = OnCheckPointEvent;
			}

			if ( coinEvents != null )
			{
				coinEvents.EventHandler = OnCoinEvent;
			}

			if ( onTriggerOutLine != null )
			{
				onTriggerOutLine.EventHandler = OnOutLineEvent;
			}
		}

		private void OnOutLineEvent()
		{
			UIManager.Instance.StopTimeLab();
		}

		private void OnCheckPointEvent()
		{
			if ( checkPointEvents.m_Event is not SOCheckPointEvent soCheckPointEvent ) return;

			var param = soCheckPointEvent.param;

			if (param > numCheckPoints) return;

			if ( param > CurrentCheckpoint )
			{
				CurrentCheckpoint = param;
				UIManager.Instance.UpdateCheckPoint(CurrentCheckpoint);
			}
		}

		private void OnCoinEvent()
		{
			currentCoin++;
			UIManager.Instance.UpdatePlayerInfo_Coin(currentCoin);
		}

		public float GetTotalScore()
		{
			float scoreCoins = (currentCoin / numCoins) * 10;
			float scoreTimes = ((maxTime - UIManager.Instance.timeLabManager.BestLapTime) / maxTime) * 30;

			if (numCheckPoints == 0)
				numCheckPoints = 1;

			float scoreCheckpoints = ((float)CurrentCheckpoint / (float)numCheckPoints) * 60f;

			Debug.Log($"{scoreCheckpoints} | {numCheckPoints} | {CurrentCheckpoint}");

			if ( scoreTimes < 0 )
				scoreTimes = 0;

			return scoreCoins + scoreTimes + scoreCheckpoints;
		}
	}
}
