using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour

{
    [SerializeField]
    private int health = 50;
    [SerializeField]
    private int stoneValue = 10;
    

    public void takeDamage(int damage)
    {
        health = health - damage;
        if(health <= 0)
        {
            death();
        }
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.TryGetComponent<PickaxeScript>(out PickaxeScript controller))
        {

            health = health - controller.damage;
            if (health <= 0)
            {
                controller.mindedStone(stoneValue);
                death();
            }
        }
    }

    public void death()
    {
        
        
        Destroy(gameObject);
    }
}
