using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Beanc16.Common.Mechanics.Collectables.TwoD
{
    [RequireComponent(typeof (Rigidbody2D))]
    public class Hoverable : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 10f)]
        private float hoverSpeed = 0.35f;
        
        private Vector3 topDestination;
        private Vector3 bottomDestination;
        private Vector3 curTarget;
        [SerializeField]
        [Tooltip("How far vertically the destination should be from the start")]
        private float distanceFromStart = 0.75f;
        [SerializeField]
        [Tooltip("How far the target should be from the target before it changes destinations")]
        private float distanceToTarget = 0.25f;

        private Rigidbody2D rbody;



        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            rbody = this.GetComponent<Rigidbody2D>();
            rbody.bodyType = RigidbodyType2D.Kinematic;
            
            topDestination = new Vector3(this.transform.position.x,
                                        this.transform.position.y + distanceFromStart);
            bottomDestination = new Vector3(this.transform.position.x,
                                            this.transform.position.y);
            curTarget = topDestination;
        }



        private void FixedUpdate()
        {
            Hover();
        }

        private void Hover()
        {
            MoveToNewPosition();
            TryChangeTargetDestination();
        }

        private void MoveToNewPosition()
        {
            Vector3 newPos = Vector3.Lerp(this.transform.position,
                                        curTarget,
                                        hoverSpeed * Time.deltaTime);
            rbody.MovePosition(newPos);
        }

        private void TryChangeTargetDestination()
        {
            float distToTarget = Vector3.Distance(this.transform.position, curTarget);

            if (distToTarget <= distanceToTarget)
            {
                if (curTarget == topDestination)
                {
                    curTarget = bottomDestination;
                }

                else
                {
                    curTarget = topDestination;
                }
            }
        }
    }
}
