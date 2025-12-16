using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
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
		[SerializeField] private Slider checkPointSlider;
		[SerializeField] private TextMeshProUGUI checkPointText;

		[Header("TimeLab")]
		[SerializeField] public TimeLabManager timeLabManager;
	    [SerializeField] private Button startButton;

	    [Header("Score")]
	    [SerializeField] private TextMeshProUGUI scoreText;
	    [SerializeField] private TextMeshProUGUI coinEarned;

		[Header("Team Info")]
		[SerializeField] private GameObject teamInfoPanel;
	    [SerializeField] private TextMeshProUGUI teamInfoText;
		[SerializeField] private TMP_Dropdown teamDropdown;

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

	        startButton.onClick.AddListener( () =>
	        {
		        PlayerManager.Instance.IsStartGame = true;
		        startButton.gameObject.SetActive( false );
		        timeLabManager.Started = true;
	        });

	        UpdateButton();
	        UpdatePlayerInfo_Segment( false );
	        scoreText.text = "";
        }

	    private void Update()
	    {
		    if ( timeLabManager.Started )
		    {
				scoreText.text = GameManager.Instance.GetTotalScore().ToString( CultureInfo.InvariantCulture );
		    }
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
	        checkPointSlider.DOValue( (float)checkPoint / 10f, 1f ).SetEase( Ease.InOutSine );
	        checkPointText.text = $"Point: {checkPoint}";
		}

        public void StopTimeLab()
        {
	        timeLabManager.Started = false;
        }

        public void UpdatePlayerInfo_Coin(int coin)
		{
	        coinEarned.text = $"Coin: {coin}";
		}

        public void UpdatePlayerInfo_Segment(bool isSegment)
		{
			// isUseSegment.text = $"UseSegment: {isSegment}";
		}

		public void OnAcceptInput()
		{
			teamInfoText.text = teamDropdown.options[teamDropdown.value].text;
			teamInfoPanel.SetActive( false );
		}
    }
}
