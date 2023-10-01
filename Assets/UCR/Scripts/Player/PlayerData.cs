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

	    private static readonly object Gate = new object();

	    public VehicleStage vehicleStage;
	    public ImageData rawImageRes;
	    public ImageData segmentImageRes;

	    public VehicleStage GetPlayerState()
	    {
		    var result = new VehicleStage();

		    if ( OnPlayerState != null )
		    {
			    IAsyncResult ar = OnPlayerState.BeginInvoke( new AsyncCallback( (ar) =>
			    {
				    result = OnPlayerState.EndInvoke( ar );
			    } ), null );

			    ar.AsyncWaitHandle.WaitOne();
		    }

		    return result;
	    }

	    public ImageData GetRawImage()
	    {
		    rawImageRes = new ImageData();

		    if ( OnRawImage != null )
		    {
			    IAsyncResult ar = OnRawImage.BeginInvoke( new AsyncCallback( (ar) =>
			    {
				    rawImageRes = OnRawImage.EndInvoke( ar );
			    } ), null );

			    ar.AsyncWaitHandle.WaitOne();
		    }

		    return rawImageRes;
	    }

	    public ImageData GetSegmentImage()
	    {
		    segmentImageRes = new ImageData();

		    if ( OnRawImage != null )
		    {
			    IAsyncResult ar = OnRawImage.BeginInvoke( new AsyncCallback( (ar) =>
			    {
				    segmentImageRes = OnRawImage.EndInvoke( ar );
			    } ), null );

			    ar.AsyncWaitHandle.WaitOne();
		    }

		    return segmentImageRes;
	    }
    }
}
