using System;
using Entities;
using Level;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class GameLevel : MonoBehaviour
    {
        public struct EndData
        {
            public int totalGathered;
            public int totalCollectible;
            public float totalTime;
        }
        
        public static UnityEvent<EndData> OnLevelFinished = new();
        public static UnityEvent OnLevelCountdown = new();

        [SerializeField] private LevelArea[] areas;
        [SerializeField] private AreaFocus focus;
        [SerializeField] private LevelArea firstArea;
        [SerializeField] private EndGoal goal;
        [SerializeField] private PlayerController player;

        private float startTime;
        private Vector3 startPoint;

        public void Awake()
        {
            startPoint = player.transform.position;
            Initialize();
        }

        public void Initialize()
        {
            foreach (var area in areas)
            {
                area.Initialize();
                area.onEnter.RemoveAllListeners();
                area.onEnter.AddListener(OnEnterArea);
            }
            
            focus.MoveTo(firstArea.transform.position);
            player.transform.position = startPoint;
            player.SetRespawnPoint(firstArea.GetClosestSafePoint(player.transform.position));

            goal.onGoal += OnGoalReached;
            
            player.SetEnable(false);

            Invoke(nameof(StartGame), 3f);
            OnLevelCountdown?.Invoke();
        }

        void StartGame()
        {
            startTime = Time.time;
            player.SetEnable(true);
        }

        private void OnGoalReached()
        {
            goal.onGoal -= OnGoalReached;

            player.SetEnable(false);
            
            int collected, total;
            collected = total = 0;
            
            foreach (var area in areas)
            {
                var (c, t) = area.GetTotalCollectibleCount();
                collected += c;
                total += t;
            }

            var totalTime = Time.time - startTime;
            
            OnLevelFinished?.Invoke(new EndData()
                {totalCollectible = total, totalGathered = collected, totalTime = totalTime});
        }

        private void OnEnterArea(LevelArea area)
        {
            focus.MoveTo(area.transform.position);
        }
    }
}