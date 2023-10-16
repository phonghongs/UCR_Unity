using UnityEngine;
using VoidCEEC.Core;

namespace VoidCEEC.Shared
{
	[CreateAssetMenu(fileName = nameof(SOCheckPointEvent),
		menuName = "CEEC/" + nameof(SOCheckPointEvent))]
	public class SOCheckPointEvent : AbstractGameEvent
	{
		public int param;

		public override void Reset()
		{
			param = 0;
		}

		public void SetParam(int paramValue)
		{
			param = paramValue;
		}
	}
}
