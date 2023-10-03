using UnityEngine;
using VoidCEEC.Core;
using VoidCEEC.Shared;

namespace VoidCEEC.UCR.Player
{
	[CreateAssetMenu(fileName = nameof(SoPlayerControlEvent),
		menuName = "CEEC/" + nameof(SoPlayerControlEvent))]
	public class SoPlayerControlEvent : AbstractGameEvent
	{
		public float speed;
		public float steerAngle;

		public override void Reset()
		{
			speed = 0;
			steerAngle = 0;
		}

		public void SetController(float speed, float steerAngle)
		{
			this.speed = speed;
			this.steerAngle = steerAngle;
		}
	}
}
