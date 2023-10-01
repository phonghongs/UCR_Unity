using System;
using UnityEngine;
using VoidCEEC.Shared;
using VoidCEEC.UCR.Player;

namespace VoidCEEC.UCR.Test
{
	public class PlayerInstance : MonoBehaviour
	{
		[SerializeField] public PlayerData playerData;
		[SerializeField] public float speed;
		[SerializeField] public float steerAngle;

		private void Start()
		{
			playerData.OnPlayerState += GetState;
		}

		private VehicleStage GetState()
		{
			Debug.Log("OK");
			return new VehicleStage()
			{
				Angle = steerAngle,
				Speed = speed
			};
		}
	}
}
