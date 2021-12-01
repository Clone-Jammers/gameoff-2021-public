using System;
using System.Collections;
using UnityEngine;

namespace Components
{
    public class DecayingObject : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private float lifeTime;
        #pragma warning restore 649

        private void Start()
        {
            StartCoroutine(DecayRoutine());
            
            
            IEnumerator DecayRoutine()
            {
                yield return new WaitForSeconds(lifeTime);
                Destroy(gameObject);
            }
        }
    }
}