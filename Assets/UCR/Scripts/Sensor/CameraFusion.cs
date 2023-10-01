using UnityEngine;
using UnityEngine.Rendering;

namespace VoidCEEC.UCR.Sensor
{
    public class CameraFusion : MonoBehaviour
    {
	    // pass configuration
	    [SerializeField] private Camera rawCamera;
	    [SerializeField] private Camera segmentCamera;

	    [SerializeField] private Shader uberReplacementShader;
	    [SerializeField] private Shader opticalFlowShader;
	    [SerializeField] private float opticalFlowSensitivity = 1.0f;
	    [SerializeField] private Vector2 resolution = new Vector2( 640, 480 );

	    [SerializeField] private bool supportsAntialiasing;
	    [SerializeField] private bool needsRescale;

	    // cached materials
	    private Material _opticalFlowMaterial;
	    private static readonly int Sensitivity = Shader.PropertyToID( "_Sensitivity" );
	    private static readonly int ObjectColor = Shader.PropertyToID("_ObjectColor");
	    private static readonly int CategoryColor = Shader.PropertyToID("_CategoryColor");

	    public enum CameraType
	    {
		    Raw,
		    Segment
	    }

	    private void Start()
        {
	        if ( rawCamera == null || segmentCamera == null )
	        {
		        Debug.LogError("[CameraFusion]: CameraFusion requires rawCamera and segmentCamera to be set.]");
		        return;
	        }

	        InitSegmentCamera();
        }

        private void InitSegmentCamera()
        {
	        // default fallbacks, if shaders are unspecified
	        if ( !uberReplacementShader )
		        uberReplacementShader = Shader.Find( "Hidden/UberReplacement" );

	        if ( !opticalFlowShader )
		        opticalFlowShader = Shader.Find( "Hidden/OpticalFlow" );

	        // cache materials and setup material properties
	        if ( !_opticalFlowMaterial || _opticalFlowMaterial.shader != opticalFlowShader )
		        _opticalFlowMaterial = new Material( opticalFlowShader );
	        _opticalFlowMaterial.SetFloat( Sensitivity, opticalFlowSensitivity );

	        segmentCamera.RemoveAllCommandBuffers();

	        SetupCameraWithReplacementShader(
		        segmentCamera,
		        uberReplacementShader,
		        Color.black
	        );
	        OnSceneChange();
        }

        public byte[] GetImage(CameraType cameraType)
		{
	        var cam = (cameraType == CameraType.Raw) ? rawCamera : segmentCamera;
	        var width = (int)resolution.x;
	        var height = (int)resolution.y;

	        return GetImageResult(cam, width, height, supportsAntialiasing, needsRescale);
		}

        private static byte[] GetImageResult(Camera cam, int width, int height, bool supportsAntialiasing, bool needsRescale)
        {
	        var depth = 24;
	        var format = RenderTextureFormat.Default;
	        var readWrite = RenderTextureReadWrite.Default;
	        var antiAliasing = (supportsAntialiasing) ? Mathf.Max(1, QualitySettings.antiAliasing) : 1;

	        var finalRT =
		        RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
	        var renderRT = (!needsRescale) ? finalRT :
		        RenderTexture.GetTemporary(cam.pixelWidth, cam.pixelHeight, depth, format, readWrite, antiAliasing);
	        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

	        var prevActiveRT = RenderTexture.active;
	        var prevCameraRT = cam.targetTexture;

	        // render to offscreen texture (readonly from CPU side)
	        RenderTexture.active = renderRT;
	        cam.targetTexture = renderRT;

	        cam.Render();

	        if (needsRescale)
	        {
		        // blit to rescale (see issue with Motion Vectors in @KNOWN ISSUES)
		        RenderTexture.active = finalRT;
		        Graphics.Blit(renderRT, finalRT);
		        RenderTexture.ReleaseTemporary(renderRT);
	        }

	        // read offscreen texture contents into the CPU readable texture
	        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
	        tex.Apply();

	        // encode texture into PNG
	        var bytes = tex.EncodeToPNG();

	        // restore state and cleanup
	        cam.targetTexture = prevCameraRT;
	        RenderTexture.active = prevActiveRT;

	        Object.Destroy(tex);
	        RenderTexture.ReleaseTemporary(finalRT);

	        return bytes;
        }


        private static void SetupCameraWithReplacementShader(Camera cam, Shader shader, Color clearColor)
        {
	        var cb = new CommandBuffer();
	        cam.AddCommandBuffer( CameraEvent.BeforeForwardOpaque, cb );
	        cam.AddCommandBuffer( CameraEvent.BeforeFinalPass, cb );
	        cam.SetReplacementShader( shader, "" );
	        cam.backgroundColor = clearColor;
	        cam.clearFlags = CameraClearFlags.SolidColor;
        }

        private static void OnSceneChange()
        {
	        var renderers = Object.FindObjectsOfType<Renderer>();
	        var mpb = new MaterialPropertyBlock();
	        foreach ( var r in renderers )
	        {
		        GameObject o;
		        var id = (o = r.gameObject).GetInstanceID();
		        var layer = o.layer;

		        mpb.SetColor( ObjectColor, ColorEncoding.EncodeIDAsColor( id ) );
		        mpb.SetColor( CategoryColor, ColorEncoding.EncodeLayerAsColor( layer ) );
		        r.SetPropertyBlock( mpb );
	        }
        }
    }
}
