using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.TryGetComponent<TreeScript>(out TreeScript controller))
        {
            print("triggeAxer");
            controller.takeDamage(20);

        }
        if (other.gameObject.TryGetComponent<AiController>(out AiController controller2))
        {
            print("AxeEnemy");
            controller2.TakeDamage(20);

        }
    }
}
