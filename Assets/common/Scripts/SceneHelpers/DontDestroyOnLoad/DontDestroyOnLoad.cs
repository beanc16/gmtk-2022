using UnityEngine;



namespace Beanc16.Common.SceneHelpers
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
