using UnityEngine;
using VoidCEEC.Core;

namespace VoidCEEC.Shared
{
	[CreateAssetMenu(fileName = nameof(ChangeAVModeEvent),
		menuName = "CEEC/" + nameof(ChangeAVModeEvent))]
	public class ChangeAVModeEvent: AbstractGameEvent
	{
		public override void Reset()
		{
		}
	}
}
