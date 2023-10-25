using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using VoidCEEC.Core;
using VoidCEEC.Shared;
using VoidCEEC.UCR.Manager;

namespace VoidCEEC
{
    public class CheckStopSign : MonoBehaviour
    {
	    [SerializeField] private SOCheckPointEvent onCheckPointEvent;
	    [SerializeField] private AbstractGameEvent outlineEvent;
	    [SerializeField] private int param;
	    [SerializeField] private float waitTime;
	    [SerializeField] private bool isStop;

	    private void Start()
	    {
		    isStop = false;
	    }

	    private void OnTriggerEnter(Collider other)
	    {
		    if (other.CompareTag("Player"))
		    {
			    isStop = true;
			    StartCoroutine( CheckStop() );
		    }
	    }

	    private void OnTriggerExit(Collider other)
	    {
		    if (other.CompareTag("Player"))
		    {
			    isStop = false;
		    }
	    }

	    IEnumerator CheckStop()
	    {
		    yield return new WaitForSeconds(waitTime);
		    if ( isStop )
		    {
			    onCheckPointEvent.SetParam(param);
			    onCheckPointEvent.Raise();
			    outlineEvent.Raise();
		    }
	    }
    }
}
