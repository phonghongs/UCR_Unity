using System;
using System.Threading;
using UnityEngine;
using VoidCEEC.Core;
using VoidCEEC.Shared;

namespace VoidCEEC.UCR.Player
{
	[CreateAssetMenu(fileName = nameof(PlayerData),
		menuName = "CEEC/" + nameof(PlayerData))]
    public class PlayerData : ScriptableObject
    {
	    public delegate VehicleStage PlayerState();
	    public delegate ImageData RawImage();
	    public delegate ImageData SegmentImage();

	    public event PlayerState OnPlayerState;
	    public event RawImage OnRawImage;
	    public event SegmentImage OnSegmentImage;

	    public VehicleStage GetPlayerState()
	    {
		    return OnPlayerState?.Invoke();
	    }

	    public ImageData GetRawImage()
	    {
		    return OnRawImage?.Invoke();
	    }

	    public ImageData GetSegmentImage()
	    {
		    return OnSegmentImage?.Invoke();
	    }
    }
}
