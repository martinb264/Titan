using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<AiController>(out AiController controller))
        {
            controller.TakeDamage(50);
            Destroy(gameObject);
        }

       
    }
}
