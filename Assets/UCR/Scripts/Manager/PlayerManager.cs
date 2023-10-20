using System;
using System.Collections;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VoidCEEC.Core;
using VoidCEEC.Shared;
using VoidCEEC.UCR.Player;
using VoidCEEC.UCR.Sensor;

namespace VoidCEEC.UCR.Manager
{

	public class PlayerManager : Core.Singleton<PlayerManager>
	{
		[Header("Events")]
		[SerializeField] private PlayerData playerData;
		public GenericGameEventListener onPlayerControlEvent;
		public GenericGameEventListener onTriggerOutLine;
		public GenericGameEventListener onAvModeEvent;

		[Header("Sensor")]
		[SerializeField] private CameraFusion cameraFusion;

		[Header( "PlayerState" )]
		[SerializeField] private Transform starTransform;
		[SerializeField] private float speed;
		[SerializeField] private float steerAngle;
		public bool IsStartGame { get; set; }

		[SerializeField] private PrometeoCarController prometeoCarController;

		public bool IsAvControl { get; private set; }

		private void OnEnable()
		{
			playerData.OnPlayerState += OnPlayerState;
			playerData.OnRawImage += GetRawImage;
			playerData.OnSegmentImage += GetSegmentImage;

			onPlayerControlEvent?.Subscribe();
			onAvModeEvent?.Subscribe();
			onTriggerOutLine?.Subscribe();
		}

		private void OnDisable()
		{
			playerData.OnPlayerState -= OnPlayerState;
			playerData.OnRawImage -= GetRawImage;
			playerData.OnSegmentImage -= GetSegmentImage;

			onPlayerControlEvent?.Unsubscribe();
			onAvModeEvent?.Unsubscribe();
			onTriggerOutLine?.Unsubscribe();
		}

		private void Start()
		{
			if ( onPlayerControlEvent != null )
			{
				onPlayerControlEvent.EventHandler = OnPlayerControlEvent;
			}

			if (onAvModeEvent != null)
			{
				onAvModeEvent.EventHandler = UpdateAvControlStatus;
			}

			if (onTriggerOutLine != null)
			{
				onTriggerOutLine.EventHandler = () =>
				{
					OnResetPosition(true);
				};
			}

			IsStartGame = false;

			OnResetPosition(false);
		}

		private void OnPlayerControlEvent()
		{
			if ( onPlayerControlEvent.m_Event is not SoPlayerControlEvent soPlayerControlEvent ) return;
			if (!IsStartGame) return;

			var crSpeed = soPlayerControlEvent.speed;
			var crSteerAngle = soPlayerControlEvent.steerAngle;
			prometeoCarController.SetAVCOntroller(crSpeed, crSteerAngle);
		}

		private VehicleStage OnPlayerState()
		{
			PrometeoCarController.VehicleStageCl bk = prometeoCarController.GetState();

			return new VehicleStage()
			{
				Speed = bk.crSpeed,
				Angle = bk.crSteering,
				Heading = bk.rotation.y
			};
		}

		private ImageData GetRawImage()
		{
			return new ImageData()
			{
				Image = cameraFusion.GetImage( CameraFusion.CameraType.Raw )
			};
		}

		private ImageData GetSegmentImage()
		{
			UIManager.Instance.UpdatePlayerInfo_Segment(true);

			return new ImageData()
			{
				Image = cameraFusion.GetImage( CameraFusion.CameraType.Segment )
			};
		}

		private void UpdateAvControlStatus()
		{
			IsAvControl = !IsAvControl;
			prometeoCarController.isAvController = IsAvControl;
		}

		private void UpdateAvControlStatus(bool isAvControl)
		{
			IsAvControl = isAvControl;
			prometeoCarController.isAvController = IsAvControl;
		}

		private void OnResetPosition(bool isOutline)
		{
			if ( starTransform != null )
			{
				UpdateAvControlStatus(false);

				var transform1 = prometeoCarController.transform;
				transform1.position = starTransform.position;
				transform1.rotation = starTransform.rotation;

				if ( isOutline )
				{
					StartCoroutine(ResetHelper());
				}
			}
		}

		IEnumerator ResetHelper(){
			Debug.Log("ResetHelper");


			var kinematic = prometeoCarController.GetComponent<Rigidbody>();

			kinematic.isKinematic = true;
			yield return new WaitForEndOfFrame();
			kinematic.isKinematic = false;
			// yield return new WaitForEndOfFrame();
			// kinematic.isKinematic = true;
		}
	}
}
