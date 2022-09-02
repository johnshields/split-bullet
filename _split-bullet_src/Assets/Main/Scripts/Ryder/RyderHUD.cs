using TMPro;
using UnityEngine;

namespace Player
{
    public class RyderHUD : MonoBehaviour
    {
        public TextMeshProUGUI bulletsCounter;

        private void OnGUI()
        {
            int chamberNum;
            if (GetComponent<ScottAndWalton>().chamber <= 0)
                chamberNum = 0;
            else
                chamberNum = GetComponent<ScottAndWalton>().chamber - 1;
            
            bulletsCounter.text = $"{chamberNum} / {GetComponent<ScottAndWalton>().bullets}";
        }
    }
}