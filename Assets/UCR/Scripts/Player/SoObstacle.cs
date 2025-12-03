using UnityEngine;
using VoidCEEC.Core;

namespace VoidCEEC.UCR.Player
{
	[CreateAssetMenu(fileName = nameof(SoObstacle),
		menuName = "CEEC/" + nameof(SoObstacle))]
    public class SoObstacle : AbstractGameEvent
    {
	    public override void Reset()
	    {
	    }
    }
}
