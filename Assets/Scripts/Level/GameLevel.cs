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
        
        [SerializeField] private LevelArea[] areas;
        [SerializeField] private AreaFocus focus;
        [SerializeField] private LevelArea firstArea;
        [SerializeField] private EndGoal goal;
        [SerializeField] private PlayerController player;

        public void Awake()
        {
            foreach (var area in areas)
            {
                area.Initialize();
                area.onEnter.AddListener(OnEnterArea);
            }
            
            focus.MoveTo(firstArea.transform.position);
        }

        private void OnEnterArea(LevelArea area)
        {
            focus.MoveTo(area.transform.position);
        }
    }
}