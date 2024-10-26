using UnityEngine;

namespace DataBase.Ammo
{
    [CreateAssetMenu(fileName = "AmmoData", menuName = "AmmoData", order = 0)]
    public class AmmoData : ScriptableObject
    {
        public string ammoName;
        public float ammoSpeed;
        public GameObject ammo;
        public float damage;
    }
}