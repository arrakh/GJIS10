using Player;

namespace Entities
{
    public interface IInteractable
    {
        public void OnInteract(PlayerController interactor);
    }
}