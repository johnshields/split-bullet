using TMPro;
using UnityEngine;

namespace Player
{
    public class RyderHUD : MonoBehaviour
    {
        public TextMeshProUGUI bulletsCounter;

        private void OnGUI()
        {
            bulletsCounter.text = $"BULLETS: {GetComponent<ScottAndWalton>().bullets}";
        }

    }
}
