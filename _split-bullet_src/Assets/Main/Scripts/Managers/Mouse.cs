using UnityEngine;

namespace Managers
{
    public class Mouse : MonoBehaviour
    {
        private void Awake()
        {
            MouseStatus(true, true);
        }

        private void MouseStatus(bool vis, bool locked)
        {
            Cursor.visible = vis;
            if (locked) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }
    }
}