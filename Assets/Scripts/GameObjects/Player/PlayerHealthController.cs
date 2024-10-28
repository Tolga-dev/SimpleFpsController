using System;
using UnityEngine;

namespace GameObjects.Player
{            

    
    [Serializable]
    public class PlayerHealthController
    {
        // health
        public float maxHealth = 100f;
        public float currentHealth;
        public float regenRate = 5f;        // Health regenerated per second
        public float regenDelay = 3f;       // Delay after taking damage before regen starts
        private float regenTimer = 0f;      // Tracks time since last damage

        public void Starter()
        {
            currentHealth = maxHealth;
        }
        
        public void RegenerateHealth()
        { 
            if (currentHealth < maxHealth && regenTimer >= regenDelay)
            {
                currentHealth += regenRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't exceed max
            }
            else
            {
                regenTimer += Time.deltaTime;  // Increase timer if delay hasn't passed
            }
        }
        
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            regenTimer = 0f;  // Reset timer when taking damage
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // Implement death logic here
            Debug.Log("Player has died!");
        }
        
    }
}