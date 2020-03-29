using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    //https://www.youtube.com/watch?v=5C6zr4Q5AlA
    //https://www.youtube.com/watch?v=MKOc8J877tI
    public SteamVR_Action_Vector2 input;
    public float speed = 2;
    private CharacterController characterController;
    public Transform head;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //characterController.center = new Vector3(head.transform.position.x, 0.75f, head.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction,Vector3.up) - new Vector3(0,9.81f,0) * Time.deltaTime);
        //characterController.center = new Vector3(head.transform.position.x, 0.75f, head.transform.position.z);
    }
}
