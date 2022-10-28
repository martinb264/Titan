using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public float score;

    public HealthBar healthBar;

    public Text text;
    public Text DeathText;

    private void Start()
    {
        health = maxHealth;
        score = 0;
        healthBar.SetMaxHealth(((int)maxHealth));
        resetScore();
        DeathText.gameObject.SetActive(false);
    }

    private void takeDamage(float damage)
    {
        health -= damage;
        healthBar.SetHealth(((int)health)); 
        if(health <= 0)
        {
            DeathText.gameObject.SetActive(true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<AiController>(out AiController controller))
        {
            takeDamage(controller.attack);
        }
    }
    public void updateScore()
    {
        score += 10;
        text.text = score.ToString();
    }

    public void resetScore()
    {
        text.text = score.ToString();
    }
}
