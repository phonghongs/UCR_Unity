using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoidCEEC.UCR.Manager
{
	public class TimeLabManager : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI timeLabText;
		public bool Started { get; set; }
		public float BestLapTime { get; set; }

		public float maxTimePerLap;
		public float currentLapTime;

		private void Start()
		{
			currentLapTime = 0f;
			BestLapTime = 0f;
		}

		private void Update()
		{
			if ( Started )
			{
				currentLapTime += Time.deltaTime;
				BestLapTime = currentLapTime;

				if ( timeLabText != null )
				{
					TimeSpan time = TimeSpan.FromSeconds(BestLapTime);
					timeLabText.text = time.ToString(@"mm\:ss\.fff"); // 00:03:48
				}
			}
		}
	}
}
