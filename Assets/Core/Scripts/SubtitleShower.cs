using System.Collections;
using Nocci.Zayebunny.Extensions;
using TMPro;
using UnityEngine;

namespace Nocci
{
    public class SubtitleShower : Singleton<SubtitleShower>
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Doozy.Runtime.UIManager.Containers.UIContainer container;


        public void Show(string text, float delay)
        {
            this.text.text = text;
            ShowRoutine(delay).Run();
        }

        IEnumerator ShowRoutine(float delay)
        {
            container.Show();
            yield return new WaitForSeconds(delay);
            container.Hide();
        }
    }
}