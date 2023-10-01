namespace VoidCEEC.Shared
{
	/*
	Client                                  ServerMessage
	______________________________________________________________
	CMD :   Function                        CMD :   Function
	1   :   SetController                   1   :   VehicleStage
	2   :   GetImage                        2   :   OriginalImage
	3   :   GetImage                        3   :   SegmentImage
*/

	public class SetController
	{
		public short Cmd {
			get;
			set;
		}
		public float Speed {
			get;
			set;
		}
		public float Angle {
			get;
			set;
		}
	}

	public class GetImage
	{
		public short Cmd {
			get;
			set;
		}
	}

	public class VehicleStage
	{
		public short Cmd {
			get;
			set;
		}
		public float Speed {
			get;
			set;
		}
		public float Angle {
			get;
			set;
		}
		public float Heading {
			get;
			set;
		}
	}

	public class ImageData
	{
		public short Cmd {
			get;
			set;
		}
		public byte[] Image {
			get;
			set;
		}
	}
}
