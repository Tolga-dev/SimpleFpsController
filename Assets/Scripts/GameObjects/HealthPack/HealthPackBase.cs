using GameObjects.Player;
using UnityEngine;

namespace GameObjects.HealthPack
{
    public class HealthPackBase : MonoBehaviour
    {
        public float playerHealth;
        
        public void PickupHealth(SimpleFPSController player)
        {
            player.playerHealthController.currentHealth += playerHealth;
            player.playerHealthController.currentHealth =      
                Mathf.Clamp(player.playerHealthController.currentHealth, 0, player.playerHealthController.maxHealth); // Cap health at maxHealth
            Debug.Log("Picked up health pack!");
        }
    }
}