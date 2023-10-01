using System;
using UnityEngine;
using VoidCEEC.Core;
using VoidCEEC.Shared;
using VoidCEEC.UCR.Player;
using VoidCEEC.UCR.Sensor;

namespace VoidCEEC.UCR.Manager
{
	public class PlayerManager : MonoBehaviour
	{
		[Header("Events")]
		[SerializeField] private PlayerData playerData;
		public GenericGameEventListener onPlayerControlEvent;

		[Header("Sensor")]
		[SerializeField] private CameraFusion cameraFusion;

		[Header("PlayerState")]
		[SerializeField] private float speed;
		[SerializeField] private float steerAngle;

		private void OnEnable()
		{
			playerData.OnPlayerState += OnPlayerState;
			playerData.OnRawImage += GetRawImage;
			playerData.OnSegmentImage += GetSegmentImage;
		}

		private void OnDisable()
		{
			playerData.OnPlayerState -= OnPlayerState;
			playerData.OnRawImage -= GetRawImage;
			playerData.OnSegmentImage -= GetSegmentImage;
		}

		private void Start()
		{
			onPlayerControlEvent.EventHandler = OnPlayerControlEvent;
		}

		int framecount = 0;
		private void Update()
		{
			if (framecount % 30 == 0) {
				playerData.rawImageRes = GetRawImage();
				playerData.segmentImageRes = GetSegmentImage();
				playerData.vehicleStage = OnPlayerState();
			}
			framecount ++;
		}

		private void OnPlayerControlEvent()
		{
			if ( onPlayerControlEvent.m_Event is not SoPlayerControlEvent soPlayerControlEvent ) return;

			var crSpeed = soPlayerControlEvent.speed;
			var crSteerAngle = soPlayerControlEvent.steerAngle;
			Debug.Log($"[OnPlayerControlEvent]: {crSpeed} : {crSteerAngle}");
		}

		private VehicleStage OnPlayerState()
		{
			return new VehicleStage()
			{
				Speed = speed,
				Angle = steerAngle,
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
			return new ImageData()
			{
				Image = cameraFusion.GetImage( CameraFusion.CameraType.Segment )
			};
		}
	}
}
