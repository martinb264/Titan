using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeScript : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    
    public int damage = 20;

    public void OnTriggerEnter(Collider other)
    {
        /*
        print("trigger");
        if (other.gameObject.TryGetComponent<RockScript>(out RockScript controller))
        {
            controller.takeDamage(20);

        }
        */
        if (other.gameObject.TryGetComponent<AiController>(out AiController controller2))
        {
            controller2.TakeDamage(10);

        }
    }

    public void mindedStone(int value)
    {
        playerController.minedStone(value);
    }
}
