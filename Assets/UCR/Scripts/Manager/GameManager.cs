using UnityEngine;
using VoidCEEC.Core;
using VoidCEEC.Shared;

namespace VoidCEEC.UCR.Manager
{
	public class GameManager : Singleton<GameManager>
	{
		[Header("CheckPoint")]
		[SerializeField] private GenericGameEventListener checkPointEvents;
		public int CurrentCheckpoint { get; private set; }

		private void OnEnable()
		{
			checkPointEvents.Subscribe();
		}

		private void OnDisable()
		{
			checkPointEvents.Unsubscribe();
		}

		private void Start()
		{
			CurrentCheckpoint = 0;

			if ( checkPointEvents != null )
			{
				checkPointEvents.EventHandler = OnCheckPointEvent;
			}
		}

		private void OnCheckPointEvent()
		{
			if ( checkPointEvents.m_Event is not SOCheckPointEvent soCheckPointEvent ) return;

			var param = soCheckPointEvent.param;
			if ( param > CurrentCheckpoint )
			{
				CurrentCheckpoint = param;
				UIManager.Instance.UpdateCheckPoint(CurrentCheckpoint);
			}
		}
	}
}
