using DataBase.Ammo;
using UnityEngine;

namespace GameObjects.Ammo.Base
{
    public class AmmoBaseInGame : MonoBehaviour
    {
        public AmmoData ammoData;
        public float gunDamage;
        public Rigidbody rb;
        
        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log(ammoData.damage + gunDamage);
            }
        }
    }
}