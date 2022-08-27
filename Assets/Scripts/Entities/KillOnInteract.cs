using Player;
using UnityEngine;

namespace Entities
{
    public class KillOnInteract : MonoBehaviour, IInteractable
    {
        public void OnInteract(PlayerController interactor)
        {
            interactor.Kill();
        }
    }
}