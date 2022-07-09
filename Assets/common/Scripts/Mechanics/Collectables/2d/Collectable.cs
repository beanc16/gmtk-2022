using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Beanc16.Common.Mechanics.Collectables.TwoD
{
    [System.Serializable]
    public class CollectionEnterSuccessfulEvent : UnityEvent<Collectable> { }

    [System.Serializable]
    public class CollectionEnterFailedEvent : UnityEvent<Collectable> { }

    [System.Serializable]
    public class CollectionStaySuccessfulEvent : UnityEvent<Collectable> { }

    [System.Serializable]
    public class CollectionStayFailedEvent : UnityEvent<Collectable> { }

    [System.Serializable]
    public class CollectionExitSuccessfulEvent : UnityEvent<Collectable> { }

    [System.Serializable]
    public class CollectionExitFailedEvent : UnityEvent<Collectable> { }



    [RequireComponent(typeof (Collider2D))]
    [RequireComponent(typeof (Rigidbody2D))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Should this collectable automatically disable itself " +
                "when a Collector collects it?")]
        private bool disableCollectableWhenCollected = true;
        [SerializeField]
        [Tooltip("How much to increase a Collector's score by when this " +
                "object is collected")]
        private int increaseCollectorScoreBy = 10;
        
        [Space(15)]
        public CollectionEnterSuccessfulEvent OnCollectionEnterSuccessful;
        public CollectionEnterFailedEvent OnCollectionEnterFailed;
        public CollectionStaySuccessfulEvent OnCollectionStaySuccessful;
        public CollectionStayFailedEvent OnCollectionStayFailed;
        public CollectionExitSuccessfulEvent OnCollectionExitSuccessful;
        public CollectionExitFailedEvent OnCollectionExitFailed;



        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Collider2D collider = this.GetComponent<Collider2D>();
            collider.isTrigger = true;

            Rigidbody2D rbody = this.GetComponent<Rigidbody2D>();
            rbody.bodyType = RigidbodyType2D.Kinematic;
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            Collector collector = collision.gameObject.GetComponent<Collector>();

            // Hit a collector
            if (collector != null)
            {
                collector.IncreaseScore(increaseCollectorScoreBy);
                TryCallCollectionEnterSuccessfulEvent();

                if (disableCollectableWhenCollected)
                {
                    DisableCollectable();
                }
            }

            // Hit something else
            else
            {
                TryCallCollectionEnterFailedEvent();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Collector collector = collision.gameObject.GetComponent<Collector>();

            // Hit a collector
            if (collector != null)
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
            Collector collector = collision.gameObject.GetComponent<Collector>();

            // Hit a collector
            if (collector != null)
            {
                TryCallCollectionExitSuccessfulEvent();
            }

            // Hit something else
            else
            {
                TryCallCollectionExitFailedEvent();
            }
        }



        public void EnableCollectable()
        {
            this.gameObject.SetActive(true);
        }

        public void DisableCollectable()
        {
            this.gameObject.SetActive(false);
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
    }
}
