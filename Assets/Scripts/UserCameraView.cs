using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCameraView : MonoBehaviour
{
    public Transform userCar; // note: this doesnt have to always be the same car
    public float yOffset = 1f;
    


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(userCar.position.x,
                                         userCar.position.y + yOffset,
                                         userCar.position.z);
    }
}
