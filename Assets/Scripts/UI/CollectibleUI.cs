using System;
using DefaultNamespace;
using Level;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CollectibleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tempInfo;

        private void Awake()
        {
            LevelArea.OnCollectableCountUpdate.AddListener(OnUpdateCount);
        }

        private void OnUpdateCount(int count, int outOf)
        {
            Debug.Log("UPDATED COUNT");
            tempInfo.text = $"{count} / {outOf}";
        }
    }
}