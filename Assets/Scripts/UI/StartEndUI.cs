using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class StartEndUI : MonoBehaviour
    {
        [SerializeField] private GameLevel level;
        [SerializeField] private GameObject startGroup, endGroup;
        [SerializeField] private Image blackImage;
        [SerializeField] private TextMeshProUGUI countdownText, endText;
        [SerializeField] private Button retryButton, menuButton;
        [SerializeField] private Color blackAlphaColor;
        
        private void Awake()
        {
            GameLevel.OnLevelCountdown.AddListener(OnCountdown);
            GameLevel.OnLevelFinished.AddListener(OnLevelFinished);
            
            retryButton.onClick.AddListener(OnRetry);
            menuButton.onClick.AddListener(OnMenu);
        }

        private void OnMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        private void OnRetry()
        {
            Debug.Log("RE INITIALIZING LEVEL");
            endGroup.SetActive(false);
            level.Initialize();
        }

        private void OnLevelFinished(GameLevel.EndData data)
        {
            startGroup.SetActive(false);
            endGroup.SetActive(true);
            blackImage.color = Color.clear;
            
            blackImage.CrossFadeAlpha(blackAlphaColor.a, 2f, true);
            
            TimeSpan span = TimeSpan.FromSeconds(data.totalTime);

            endText.text = "Finished! \n" +
                           "<size=40%>" +
                           $"You collected {data.totalGathered} out of {data.totalCollectible} \n" +
                           $"in {span.Minutes} minutes and {span.Seconds} seconds...";
        }

        private void OnCountdown()
        {
            endGroup.SetActive(false);
            StartCoroutine(CountdownEnumerator());
        }

        IEnumerator CountdownEnumerator()
        {
            startGroup.SetActive(true);
            blackImage.color = blackAlphaColor;

            var count = 3;
            while (count > 0)
            {
                countdownText.text = $"Starting in {count}...";
                count--;
                yield return new WaitForSeconds(1f);
            }
            
            countdownText.text = $"START!!!";
            blackImage.CrossFadeAlpha(0f, 0.5f, true);
            yield return new WaitForSeconds(0.5f);
            startGroup.SetActive(false);
        }
    }
}