using Player;
using UnityEngine;

namespace Collectables
{
    public class BulletsBox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.CompareTag("Player") &&
                obj.GetComponent<ScottAndWalton>().bullets < obj.GetComponent<ScottAndWalton>().maxBullets)
            {
                obj.GetComponent<ScottAndWalton>().fillUpBullets = true;
                obj.GetComponent<ScottAndWalton>().FillUpBullets(6);
                obj.GetComponent<RyderSFX>().BulletsPickup(.6f); // TODO - change to different SFX Script.
            }
        }

        private void OnTriggerExit(Collider obj)
        {
            if (obj.CompareTag("Player"))
                obj.GetComponent<ScottAndWalton>().fillUpBullets = false;
        }
    }
}