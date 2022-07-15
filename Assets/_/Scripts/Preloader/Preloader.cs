using _.Scripts.Core;
using Cysharp.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _.Scripts.Preloader
{
    public class Preloader : MonoBehaviour
    {
        private async UniTaskVoid Start()
        {
            await UniTask.WaitUntil(() => 
                    RuntimeManager.HaveAllBanksLoaded, 
                    cancellationToken: this.GetCancellationTokenOnDestroy());
            SceneManager.LoadScene(Constants.MainScene);
        }
    }
}
