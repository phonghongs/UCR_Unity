using System;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

		[SerializeField] private PrometeoCarController prometeoCarController;
		[SerializeField] private bool isAVControl;

		private void OnEnable()
		{
			playerData.OnPlayerState += OnPlayerState;
			playerData.OnRawImage += GetRawImage;
			playerData.OnSegmentImage += GetSegmentImage;

			onPlayerControlEvent?.Subscribe();
		}

		private void OnDisable()
		{
			playerData.OnPlayerState -= OnPlayerState;
			playerData.OnRawImage -= GetRawImage;
			playerData.OnSegmentImage -= GetSegmentImage;

			onPlayerControlEvent?.Unsubscribe();
		}

		private void Start()
		{
			if ( onPlayerControlEvent != null )
			{
				onPlayerControlEvent.EventHandler = OnPlayerControlEvent;
			}
		}

		private void OnPlayerControlEvent()
		{
			if ( onPlayerControlEvent.m_Event is not SoPlayerControlEvent soPlayerControlEvent ) return;

			var crSpeed = soPlayerControlEvent.speed;
			var crSteerAngle = soPlayerControlEvent.steerAngle;
			prometeoCarController.SetAVCOntroller(crSpeed, crSteerAngle);
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

		public void UpdateAvControlStatus()
		{
			prometeoCarController.isAvController = isAVControl;
		}
	}

	#if UNITY_EDITOR
	[CustomEditor(typeof(PlayerManager))]
	class PlayerManagerEditor : Editor
	{

		private SerializedProperty _isAvControlProperty;
		private PlayerManager _playerManager;

		private void OnEnable()
		{
			_isAvControlProperty = serializedObject.FindProperty("isAVControl");
			_playerManager = (PlayerManager)target;
		}

		public override void OnInspectorGUI()
		{
			DrawPropertiesExcluding(serializedObject,
				"m_Script",
				"isAVControl"
			);

			EditorGUILayout.Space();

			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.PropertyField( _isAvControlProperty );
			}
			if ( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				_playerManager.UpdateAvControlStatus();
			}

		}
	}
	#endif
}
