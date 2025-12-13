using System.Collections.Generic;
using UnityEngine;

namespace VoidCEEC.UCR.Player
{

	public class IntersectionTrigger : MonoBehaviour
	{
		[SerializeField] private List<GameObject> wall;
		[SerializeField] private bool shouldActivate = true;

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				HandleIntersection();
			}
		}

		private void HandleIntersection()
		{
			foreach (var wallObject in wall)
			{
				if (wallObject != null)
				{
					wallObject.SetActive(shouldActivate);
				}
			}
		}
	}
}
