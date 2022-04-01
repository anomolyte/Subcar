using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody theRB;

    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, dragOnGround = 3f;

    private float speedInput, turnInput;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;

    /*
    public ParticleSystem[] dustTrail;
    private float maxEmission = 2500f;
    private float emissionRate;
    */


    // Start is called before the first frame update
    void Start()
    {
        theRB.transform.parent = null;
    }

    // Update is called once per frame
    async void Update()
    {
        speedInput = 0f;

        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
        }
        else
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");


        if (grounded) // only turn car when on ground
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
                // multiplying Y axis by getaxis returns -1~+1 which reverses turning when in reverse
        }

        transform.position = theRB.transform.position;
    }

    void FixedUpdate()
    {

        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        //emissionRate = 0;

        if (grounded) // checks to see if car on ground
        {
            theRB.drag = dragOnGround;

            if (Mathf.Abs(speedInput) > 0) // if there's any input at all, then..
            {
                theRB.AddForce(transform.forward * speedInput);
                //emissionRate = maxEmission;
            }
        }
        else // else apply increased gravity to avoid floaty behaviour
        {
            theRB.drag = 0.1f;

            theRB.AddForce(Vector3.up * -gravityForce * 100f);
        }

        /*foreach(ParticleSystem part in dustTrail)
        {
            //var emissionModule = part.emission;
            //emissionModule.rateOverTime = emissionRate;
        }*/
    }
}
