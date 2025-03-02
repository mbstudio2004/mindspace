using System.Collections;
using Doozy.Runtime.Signals;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nocci
{
    public class UIButton : MonoBehaviour
    {
        public SceneReference sceneReference;
        public Doozy.Runtime.UIManager.Components.UIButton button;
        public bool isQuit;

        private void Start()
        {
            button.onClickEvent.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (isQuit)
            {
                Application.Quit();
            }
            else
            {
                Bootloader.Bootloader.Instance.StartCoroutine(LoadNewScene());
            }
        }


        private IEnumerator LoadNewScene()
        {
            Signal.Send("Scene", "Start");
            yield return new WaitForSeconds(0.4f);

            yield return SceneManager.LoadSceneAsync(sceneReference.Path, LoadSceneMode.Single);
            yield return null;
            Signal.Send("Scene", "End");
        }
    }
}