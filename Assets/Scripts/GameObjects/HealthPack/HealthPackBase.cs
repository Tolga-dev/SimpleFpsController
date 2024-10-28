using UnityEngine;

namespace GameObjects.HealthPack
{
    public class HealthPackBase : MonoBehaviour
    {
        public float playerHealth;
        
        public void PickupHealth(SimpleFPSController.SimpleFPSController player)
        {
            player.currentHealth += playerHealth;
            player.currentHealth = 
                Mathf.Clamp(player.currentHealth, 0, player.maxHealth); // Cap health at maxHealth
            Debug.Log("Picked up health pack!");
        }
    }
}