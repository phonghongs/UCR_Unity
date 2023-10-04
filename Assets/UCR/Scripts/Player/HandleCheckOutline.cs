using UnityEngine;
using VoidCEEC.Core;

namespace VoidCEEC.UCR.Player
{
    public class HandleCheckOutline : MonoBehaviour
    {
        [SerializeField] AbstractGameEvent outlineEvent;

        private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Wall"))
				outlineEvent.Raise();
		}
    }
}
