using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    

    public HealthBar healthBar;

    public Text text;
    public Text DeathText;
    public Text StoneText;
    public int stoneValue;
    public GameObject pickaxe;
    public GameObject axe;
    [SerializeField]
    private GameObject placeAbleObject;

    private void Start()
    {
        health = maxHealth;
        
        healthBar.SetMaxHealth(((int)maxHealth));
        
        DeathText.gameObject.SetActive(false);

        pickaxe.SetActive(true);
        axe.SetActive(false);
    }
    private void Update()
    {
       
        if (Input.GetButtonDown("Fire1"))
        {
           
            pickaxe.SetActive(true);
            axe.SetActive(false);
        }
        if (Input.GetButtonDown("Fire2"))
        {
           
            pickaxe.SetActive(false);
            axe.SetActive(true);
        }
        if(Input.GetButtonDown("Fire3"))
        {
            spawnStoneObject();
        }
    }

    public void takeDamage(float damage)
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
        /*
        if (collision.gameObject.TryGetComponent<AiController>(out AiController controller))
        {
            takeDamage(controller.attack);
        }
        */
    }

    public void minedStone(int value)
    {
        stoneValue += value;
        StoneText.text = stoneValue.ToString();
    }

    public void spawnStoneObject()
    {
        if (stoneValue >= 10)
        {
            stoneValue -= 10;
            StoneText.text = stoneValue.ToString();
            Instantiate(placeAbleObject, transform.position, transform.rotation);
        }
    }

}
