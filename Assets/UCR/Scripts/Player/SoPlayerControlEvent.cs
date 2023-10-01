using UnityEngine;
using VoidCEEC.Core;
using VoidCEEC.Shared;

namespace VoidCEEC.UCR.Player
{
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
