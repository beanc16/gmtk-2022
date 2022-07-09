using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;



namespace Beanc16.Common.Mechanics.Collectables.TwoD
{
    [System.Serializable]
    public class CollectorEnterSuccessfulEvent : UnityEvent<Collector> { }

    [System.Serializable]
    public class CollectorEnterFailedEvent : UnityEvent<Collector> { }

    [System.Serializable]
    public class CollectorStaySuccessfulEvent : UnityEvent<Collector> { }

    [System.Serializable]
    public class CollectorStayFailedEvent : UnityEvent<Collector> { }

    [System.Serializable]
    public class CollectorExitSuccessfulEvent : UnityEvent<Collector> { }

    [System.Serializable]
    public class CollectorExitFailedEvent : UnityEvent<Collector> { }

    [System.Serializable]
    public class ScoreChangeEvent : UnityEvent<Collector> { }



    [RequireComponent(typeof(Collider2D))]
    public class Collector : MonoBehaviour
    {
        [Tooltip("The number of collectables this Collector has " + 
                "collected")]
        public int numOfCollectables = 0;
        public int score = 0;

        [Space(15)]
        public CollectorEnterSuccessfulEvent OnCollectionEnterSuccessful;
        public CollectorEnterFailedEvent OnCollectionEnterFailed;
        public CollectorStaySuccessfulEvent OnCollectionStaySuccessful;
        public CollectorStayFailedEvent OnCollectionStayFailed;
        public CollectorExitSuccessfulEvent OnCollectionExitSuccessful;
        public CollectorExitFailedEvent OnCollectionExitFailed;
        public ScoreChangeEvent OnScoreChange;




        public void IncreaseScore(int incrementScoreBy)
        {
            score += incrementScoreBy;
        }

        public void DecreaseScore(int decrementScoreBy)
        {
            score -= decrementScoreBy;
        }

        public void ResetScore()
        {
            score = 0;
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();

            // Hit a collector
            if (collectable != null)
            {
                numOfCollectables++;

                TryCallCollectionEnterSuccessfulEvent();
                TryCallScoreChangeEvent();
            }

            // Hit something else
            else
            {
                TryCallCollectionEnterFailedEvent();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();

            // Hit a collector
            if (collectable != null)
            {
                TryCallCollectionStaySuccessfulEvent();
            }

            // Hit something else
            else
            {
                TryCallCollectionStayFailedEvent();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Collectable collectable = collision.gameObject.GetComponent<Collectable>();

            // Hit a collector
            if (collectable != null)
            {
                TryCallCollectionExitSuccessfulEvent();
            }

            // Hit something else
            else
            {
                TryCallCollectionExitFailedEvent();
            }
        }



        private void TryCallCollectionEnterSuccessfulEvent()
        {
            if (OnCollectionEnterSuccessful != null)
            {
                OnCollectionEnterSuccessful.Invoke(this);
            }
        }

        private void TryCallCollectionEnterFailedEvent()
        {
            if (OnCollectionEnterFailed != null)
            {
                OnCollectionEnterFailed.Invoke(this);
            }
        }

        private void TryCallCollectionStaySuccessfulEvent()
        {
            if (OnCollectionStaySuccessful != null)
            {
                OnCollectionStaySuccessful.Invoke(this);
            }
        }

        private void TryCallCollectionStayFailedEvent()
        {
            if (OnCollectionStayFailed != null)
            {
                OnCollectionStayFailed.Invoke(this);
            }
        }

        private void TryCallCollectionExitSuccessfulEvent()
        {
            if (OnCollectionExitSuccessful != null)
            {
                OnCollectionExitSuccessful.Invoke(this);
            }
        }

        private void TryCallCollectionExitFailedEvent()
        {
            if (OnCollectionExitFailed != null)
            {
                OnCollectionExitFailed.Invoke(this);
            }
        }

        private void TryCallScoreChangeEvent()
        {
            if (OnScoreChange != null)
            {
                OnScoreChange.Invoke(this);
            }
        }
    }
}
