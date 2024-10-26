using System.Collections.Generic;
using UnityEngine;

namespace DataBase.Ammo
{
    [CreateAssetMenu(fileName = "AmmoDataBase", menuName = "AmmoDataBase", order = 0)]
    public class AmmoDataBase : ScriptableObject
    {
        public List<AmmoData> ammoData = new List<AmmoData>();
     
        public AmmoData GetAmmoData(AmmoData getData)
        {
            return ammoData.Find(data => data == getData); 
        }
        
    }
    
}