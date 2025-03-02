using UnityEngine;

namespace Nocci
{
    public class TriggerShowLevel : MonoBehaviour
    {
        public bool showOnlyOnce = true;
        [TextArea]
        public string text;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LevelShower.Instance.Show(text);
                if (showOnlyOnce)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
