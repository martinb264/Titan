using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    [SerializeField]
    private int health = 50;

    public void takeDamage(int damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            death();
        }
    }

    public void death()
    {
        print("TreeDeath");
        Destroy(gameObject);
    }
}
