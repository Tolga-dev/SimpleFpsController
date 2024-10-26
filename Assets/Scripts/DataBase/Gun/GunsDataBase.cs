using System.Collections.Generic;
using GameObjects.Guns.Base.Modes;
using UnityEngine;

namespace DataBase.Gun
{
    [CreateAssetMenu(fileName = "GunsDataBase", menuName = "GunsDataBase", order = 0)]
    public class GunsDataBase : ScriptableObject
    {
        public List<GunData> gunData = new List<GunData>();
        
        public GunData GetGunData(GunData getData)
        {
            return gunData.Find(data => data == getData); 
        }
        
    }
}