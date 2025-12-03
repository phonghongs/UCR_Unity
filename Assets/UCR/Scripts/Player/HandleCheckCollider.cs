using System;
using UnityEngine;
using VoidCEEC.Core;

namespace VoidCEEC.UCR.Player
{
    public class HandleCheckCollider : MonoBehaviour
    {
        [SerializeField] private AbstractGameEvent outlineEvent;
        [SerializeField] private AbstractGameEvent coinEvent;
		[SerializeField] private SoObstacle obstacleEvent;

        private void OnTriggerEnter(Collider other)
		{
			if ( other.CompareTag( "Wall" ) )
			{
				Debug.Log("Wall");
				if (outlineEvent != null)
					outlineEvent.Raise();
			}

			if ( other.CompareTag( "Coin" ) )
			{
				if (coinEvent != null)
					coinEvent.Raise();

				Destroy(other.gameObject);
			}

			if ( other.CompareTag( "Obstacle" ) )
			{
				if (obstacleEvent != null)
					obstacleEvent.Raise();

				Destroy(other.gameObject);
			}
		}
    }
}
