using UnityEngine;
using UnityEngine.Assertions;


public class test : MonoBehaviour
{
    
    public float speed = 5; // units per second
    public float turnSpeed = 90; // degrees per second
    public float jumpSpeed = 8;
    public float gravity = 9.8f;
    public float vSpeed = 0; // current vertical velocity

    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(controller);
    }
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        Vector3 vel = transform.forward * Input.GetAxis("Vertical") * speed;
        
        if (controller.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
            if (Input.GetKeyDown(KeyCode.Space))
            { // unless it jumps:
                vSpeed = jumpSpeed;
            }
        }
        // apply gravity acceleration to vertical speed:
        vSpeed -= gravity * Time.deltaTime;
        vel.y = vSpeed; // include vertical speed in vel
                        // convert vel to displacement and Move the character:
        controller.Move(vel * Time.deltaTime);
    }
}
