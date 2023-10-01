using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.XR;
using VoidCEEC.Core;
using VoidCEEC.Shared;
using VoidCEEC.UCR.Player;

namespace VoidCEEC.UCR.Networks
{
    public class Socket : Singleton<Socket>
    {
		[SerializeField] private int[] remotePort;
		[SerializeField] private int basePort = 11000;
		[SerializeField] private TMP_Text portShow;

		[SerializeField] private PlayerData[] playerData;

	    private bool[] _isRunning;
	    private Thread[] _threads;
	    private TcpListener[] _tcpListener;
	    private int activePlayer;

	    private void StartSocketServer(){
		    _tcpListener = new TcpListener[activePlayer];
	        _isRunning = new bool[activePlayer];
	        _threads = new Thread[activePlayer];

	        if (activePlayer == 0)
	        {
	            Debug.LogError("Player don't exits!");
	            return;
	        }

	        var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
	        var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

	        // Create Player Port
	        remotePort = new int[activePlayer];

	        var i = 0;
	        for (int j = 0; j < activePlayer; j ++){
	            bool portOpen =  true;
	            while (i < 100) {
	                remotePort[j] = basePort + i;
	                if (tcpConnInfoArray.Any(tcpInfo => remotePort[j] == tcpInfo.LocalEndPoint.Port))
	                {
		                portOpen = false;
	                }
	                i += 1;
	                if (!portOpen)
	                    portOpen = true;
	                else
	                    break;
	            }
	            portShow.text += remotePort[j].ToString() + " ";
	            RestartServer(j);
	        }
	    }

	    private void RestartServer(int serverIndex){
		    StopListening(serverIndex);
		    _isRunning[serverIndex] = true;
		    StartCoroutine( StartListening( serverIndex ) );
		    // _threads[serverIndex] = new Thread (() => StartListening(serverIndex));
		    // _threads[serverIndex].Start();
	    }

	    IEnumerator StartListening(int serverIndex)
	    {
		    _tcpListener[serverIndex] =
			    new TcpListener( IPAddress.Any, remotePort[serverIndex] ); //System.Net.IPAddress
		    _tcpListener[serverIndex].Start();
		    // Debug.Log("Server Started at host: localhost, port "+remotePort);//

		    // Buffer for reading data
		    Byte[] bytes = new Byte[256];
		    String jsonData = null;

		    while ( _isRunning[serverIndex] )
		    {
			    // check if new connections are pending, if not, be nice and sleep 100ms
			    if ( !_tcpListener[serverIndex].Pending() )
			    {
				    yield return new WaitForSeconds( 0.1f );
			    }
			    else
			    {
				    TcpClient client = _tcpListener[serverIndex].AcceptTcpClient();
				    NetworkStream stream = client.GetStream();
				    int i = 0;
				    jsonData = null;
				    byte[] msg = null;
				    // Loop to receive all the data sent by the client.
				    while ( (i = stream.Read( bytes, 0, bytes.Length )) != 0 )
				    {
					    jsonData = System.Text.Encoding.ASCII.GetString( bytes, 0, i );
					    var myDetails = JsonConvert.DeserializeObject<SetController>( jsonData );
					    String returnMessage = "";

					    switch ( myDetails.Cmd )
					    {
						    case 185:
							    var playerState = playerData[serverIndex].GetPlayerState();
							    var vehicleStage = new VehicleStage
							    {
								    Cmd = 18520,
								    Speed = playerState.Speed,
								    Angle = playerState.Angle,
								    Heading = playerState.Heading
							    };

							    returnMessage = JsonConvert.SerializeObject( vehicleStage );
							    msg = Encoding.UTF8.GetBytes( returnMessage );
							    break;
						    case 203:
							    var rawImage = playerData[serverIndex].GetRawImage();
							    msg = rawImage.Image;
							    break;
						    case 31:
							    var segmentImage = playerData[serverIndex].GetSegmentImage();
							    msg = segmentImage.Image;
							    break;
						    default:
							    break;
					    }

					    if ( msg == null )
					    {
						    msg = new[] { (byte)0 };
					    }

					    Debug.Log( $"{msg.Length}" );
					    stream.Write( msg, 0, msg.Length );
					    yield return new WaitForSeconds( 0.02f );
				    }

				    client.Close();
				    Debug.Log( "Client closed" );
			    }
		    } // while

		    _isRunning[serverIndex] = false;
		    _tcpListener[serverIndex].Stop();
	    }

	    private void StopListening(int serverIndex){
	        _isRunning[serverIndex] = false;
	    }

	    void Start()
	    {
		    activePlayer = playerData.Length;
	        StartSocketServer();
	    }
	}
}
