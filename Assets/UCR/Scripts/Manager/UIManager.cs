using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VoidCEEC.Core;
using VoidCEEC.UCR.Manager;

namespace VoidCEEC
{
    public class UIManager : Singleton<UIManager>
    {
	    [Header("Change AV Mode")]
        [SerializeField] private Button changeAvMode;
        [SerializeField] private TextMeshProUGUI changeAvModeText;
        [SerializeField] private string avModeText = "AV Mode";
        [SerializeField] private string manualModeText = "Manual Mode";
        [SerializeField] private AbstractGameEvent onAvModeEvent;

		[Header("CheckPoint")]
		[SerializeField] private TextMeshProUGUI checkPointText;

        private void Start()
        {
	        changeAvMode.onClick.AddListener( () =>
	        {
		        if (onAvModeEvent != null)
		        {
			        onAvModeEvent.Raise();
			        UpdateButton();
		        }
	        });

	        UpdateButton();
        }

        private void UpdateButton()
		{
			if (PlayerManager.Instance.IsAvControl)
	        {
		        changeAvModeText.text = avModeText;
	        }
	        else
	        {
		        changeAvModeText.text = manualModeText;
	        }
		}

        public void UpdateCheckPoint(int checkPoint)
		{
	        checkPointText.text = $"CheckPoint: {checkPoint}";
		}
    }
}
