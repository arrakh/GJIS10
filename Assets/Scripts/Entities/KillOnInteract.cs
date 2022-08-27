using MoreMountains.Feedbacks;
using Player;
using UnityEngine;

namespace Entities
{
    public class KillOnInteract : MonoBehaviour, IInteractable
    {
        [SerializeField] private MMF_Player hurtEffect;
        
        public void OnInteract(PlayerController interactor)
        {
            interactor.Kill();
            hurtEffect.PlayFeedbacks();
        }
    }
}