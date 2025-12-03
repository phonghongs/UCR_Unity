using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		[SerializeField] private GenericGameEventListener obstacleEvent;
		[SerializeField] private int obstacleHitPenalty;

		public int CurrentCheckpoint { get; private set; }

		private void OnEnable()
		{
			checkPointEvents.Subscribe();
			coinEvents.Subscribe();
			onTriggerOutLine.Subscribe();
			obstacleEvent.Subscribe();

		}

		private void OnDisable()
		{
			checkPointEvents.Unsubscribe();
			coinEvents.Unsubscribe();
			onTriggerOutLine.Unsubscribe();
			obstacleEvent.Unsubscribe();
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

			if ( obstacleEvent != null )
			{
				obstacleEvent.EventHandler = OnObstacleEvent;
			}
		}

		private void OnOutLineEvent()
		{
			UIManager.Instance.StopTimeLab();
		}

		private void OnObstacleEvent()
		{
			obstacleHitPenalty++;
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
			float scoreCoins = (currentCoin / numCoins) * 25;
			float scoreTimes = ((maxTime - UIManager.Instance.timeLabManager.BestLapTime) / maxTime) * 25;

			if (numCheckPoints == 0)
				numCheckPoints = 1;

			float scoreCheckpoints = ((float)CurrentCheckpoint / (float)numCheckPoints) * 50f;

			if ( scoreTimes < 0 )
				scoreTimes = 0;

			float scoreObstacles = obstacleHitPenalty * -10;

			return scoreCoins + scoreTimes + scoreCheckpoints + scoreObstacles;
		}

		private void Update()
		{
			if ( Input.GetKeyDown(KeyCode.Escape) )
			{
				OnOutLineEvent();
				PlayerManager.Instance.OnResetPosition(true);
			}

			if ( Input.GetKeyDown( KeyCode.R ) )
			{
				string currentSceneName = SceneManager.GetActiveScene().name;
				SceneManager.LoadScene(currentSceneName);
			}
		}
	}
}
