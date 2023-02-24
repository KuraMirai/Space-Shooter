using Managers;
using UnityEngine;

namespace Bullets
{
    public class BulletsParent : MonoBehaviour
    {
        private void Awake()
        {
            EventManager.StartListening(Constants.PlayerDiedEvent, ClearChildren);
            EventManager.StartListening(Constants.GoHome, ClearChildren);
            EventManager.StartListening(Constants.LevelCompleted, ClearChildren);
            EventManager.StartListening(Constants.StartGame, ClearChildren);
        }

        private void ClearChildren()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
