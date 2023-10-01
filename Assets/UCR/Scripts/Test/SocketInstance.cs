using System;
using UnityEngine;
using VoidCEEC.UCR.Player;

namespace VoidCEEC.UCR.Test
{
	public class SocketInstance : MonoBehaviour
	{
		[SerializeField] private PlayerData playerData;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.T))
			{
				var res = playerData.GetPlayerState();
				Debug.Log($"[State]: {res.Speed} : {res.Angle}");
			}
		}
	}
}
