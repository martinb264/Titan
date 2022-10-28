using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    
    

    CharacterController characterController;
    Vector3 moveVector;
    [SerializeField]
    float speed = 10f;


    // Start is called before the first frame update
    void Start()
    {


        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        characterController.Move(moveVector*speed*Time.deltaTime);
    }
    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        moveVector = new Vector3(direction.x, 0, direction.y);
    }
}
