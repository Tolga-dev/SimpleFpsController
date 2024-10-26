using DataBase.Ammo;
using UnityEngine;

namespace GameObjects.Ammo.Base
{
    public class AmmoBaseInGame : MonoBehaviour
    {
        private AmmoData ammoData;
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(ammoData.damage);
            }
        }
    }
}