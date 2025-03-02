using UnityEngine;

namespace Nocci.UI
{
    public class CursorUnlocker : MonoBehaviour
    {
        private void OnEnable()
        {
            CursorLocker.AddCursorUnlocker(this);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDisable()
        {
            CursorLocker.RemoveCursorUnlocker(this);
        }
    }
}