using System.Collections;
using Doozy.Runtime.Signals;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nocci
{
    public class TriggerScene : MonoBehaviour
    {
        public SceneReference sceneReference;
        public bool additive;
        public bool unloadCurrentScene;

        IEnumerator LoadNewScene()
        {
            if (unloadCurrentScene)
            {
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                yield break;
            }

            if (!additive)
            {
                Signal.Send("Scene", "Start");
                yield return new WaitForSeconds(0.4f);
            }

            yield return SceneManager.LoadSceneAsync(sceneReference.Path,
                additive ? LoadSceneMode.Additive : LoadSceneMode.Single);

            if (!additive)
            {
                yield return null;
                Signal.Send("Scene", "End");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Bootloader.Bootloader.Instance.StartCoroutine(LoadNewScene());
            }
        }
    }
}