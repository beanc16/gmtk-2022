using System;
using _.Scripts.Core;
using UnityEngine;

namespace _.Scripts.World
{
    public class DoorTrigger : MonoBehaviour
    {
        private Action triggerAction;
        
        public void OnTriggerAction(Action onTriggerAction)
        {
            triggerAction = onTriggerAction;
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag(Constants.TagPlayer)) return;

            triggerAction.Invoke();
        }
    }
}