using System.Collections;
using Nocci.Zayebunny.Extensions;
using TMPro;
using UnityEngine;

namespace Nocci
{
    public class LevelShower : Singleton<LevelShower>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Doozy.Runtime.UIManager.Containers.UIContainer container;
        [SerializeField] private float delay;


        public void Show(string text)
        {
            this.text.text = text;
            ShowRoutine().Run();
        }

        IEnumerator ShowRoutine()
        {
            container.Show();
            yield return new WaitForSeconds(delay);
            container.Hide();
        }
    }
}