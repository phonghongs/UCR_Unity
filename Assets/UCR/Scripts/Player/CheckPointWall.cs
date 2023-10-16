using UnityEngine;
using VoidCEEC.Shared;

namespace VoidCEEC.UCR.Player
{
    public class CheckPointWall : MonoBehaviour
    {
        [SerializeField] private SOCheckPointEvent onCheckPointEvent;
        [SerializeField] private int param;
        private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				onCheckPointEvent.SetParam(param);
				onCheckPointEvent.Raise();
			}
		}
    }
}
