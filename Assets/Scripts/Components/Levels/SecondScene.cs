using System;
using Components.Enemies;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Components.Levels
{
    public class SecondScene : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Rigidbody exitDoor;
        [SerializeField] private int killCount;
        #pragma warning restore 649

        private int _killCount;

        private void Awake()
        {
            TPoser.Killed += OnKillEnemy;
        }

        private void OnDestroy()
        {
            TPoser.Killed -= OnKillEnemy;
        }

        private void OnKillEnemy()
        {
            _killCount++;
            if (_killCount == killCount)
            {
                var openedPosition = exitDoor.transform.position - new Vector3(0, 3.5f, 0);
                exitDoor.transform.DOMove(openedPosition, 0.5f).SetEase(Ease.InSine);
            }
            else if (_killCount == killCount - 1)
            {
                GameManager.Instance.LoadLevel3();
            }
        }
    }
}