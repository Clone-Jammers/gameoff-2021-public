using Managers;
using UnityEngine;

namespace Components.Levels
{
    public class LastScene : MonoBehaviour
    {
        private bool isCalled = false;

        public void LoadNextLevel()
        {
            if (!isCalled)
            {
                isCalled = true;
                GameManager.Instance.LoadMenu();
            }
        }
    }
}