using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    public GameObject hmd;
    [SerializeField] private float leftTriggerValue;    
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isIndexTriggerDown;
    [SerializeField] private bool isLeftTriggerDown;
    [SerializeField] private bool isRightTriggerDown;

    public GameObject player;

    public GameObject leftEye;

    public GameObject rightEye;


    private float forceBuildUp;

    private float forceBuildUpFly;

    private float maxForce;

    private float maxForceFly;

    private Vector3 flatVector;

    private Vector3 tmp;

    private Vector3 upVector;

    public bool preventMovement;

    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public SelectionTaskMeasure selectionTaskMeasure;

    public GameObject startingArea;
    public GameObject startingAreaExplosion;
    public GameObject startingAreaRadio;
    public GameObject leftVignette;
    public GameObject rightVignette;
    public LayerMask layerTerrain;
    public enum FlyMethode { HMD, Controller };
    public FlyMethode flyMethode = FlyMethode.HMD;

    public float force = 10.0f;
    public float targetHeight = 10.0f;
    // private float minHMDHeight = 1.056945f;
    //private float maxHMDHeight = 1.056945f;

    //private float minHMDLocalHeight = 1.1f;
    private float minHMDLocalHeight;
    //private float maxHMDLocalHeight = 1.7f;
    private float maxHMDLocalHeight;

    public GameObject rightHand;

    private float reducingHorizontalVelocityTimeMax = 8f;
    private float reducingHorizontalVelocityTimer = 8f;

    void Start()
    {
        maxForce = 10;
        maxForceFly = 5;
        forceBuildUp = 0.02f;
        forceBuildUpFly = 0.02f;
        flatVector = new Vector3(1,0,1);
        upVector = new Vector3(0,1,0);
        preventMovement = false;
        leftVignette.SetActive(true);
        rightVignette.SetActive(true);
    }

    void FixedUpdate()
    {
        if (selectionTaskMeasure.grabLeft.isInPortal || selectionTaskMeasure.grabRight.isInPortal)
        {
            return;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();

        /*float currentHeight = hmd.transform.position.y;

        if (currentHeight < targetHeight)
        {
            // If the player is below the target height, apply an upward force
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * force);
        }
        else if (currentHeight > targetHeight)
        {
            // If the player is above the target height, apply a downward force
            player.GetComponent<Rigidbody>().AddForce(Vector3.down * 9.8f);
        }
        else
        {
            // If the player is at the target height, apply a force to counteract gravity
            player.GetComponent<Rigidbody>().AddForce(-Physics.gravity);
        }*/


        if (isLeftTriggerDown)
        {
            // forceBuildUp = -1.3f * Physics.gravity * Time.deltaTime;
            /*
            if (!isLeftTriggerDown)
            {
                isLeftTriggerDown = true;

            }
            else
            {
                // forceBuildUp += 0.01f * Time.deltaTime;
                if (forceBuildUp > maxForce)
                {
                    forceBuildUp = maxForce;
                }
            }*/

            tmp = Vector3.Scale(hmd.transform.forward.normalized, flatVector);
            //offset = Vector3.Scale(tmp, -1.3f * Physics.gravity * Time.deltaTime);

            // parkourCounter.Log(tmp.ToString());
            // tmp = Vector.Multiply(hmd.transform.forward.normalized * flatVector);

            // TODO: FIXME: Temporary disabled, trying moving to FixedUpdate()
            offset += tmp * 14f * Time.deltaTime;
            player.GetComponent<Rigidbody>().AddForce(offset, ForceMode.Impulse);

            selectionTaskMeasure.scoreText.text = offset.ToString();
            parkourCounter.Log("tmp FixedUpdate: " + tmp.ToString());
            parkourCounter.Log("offset FixedUpdate: " + offset.ToString());
        }
        else 
        {
            if (reducingHorizontalVelocityTimer > 0f)
            {
                float t = (reducingHorizontalVelocityTimeMax - reducingHorizontalVelocityTimer) / reducingHorizontalVelocityTimeMax;
                Vector3 newVelocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, 0), t);
                parkourCounter.Log("reducingHorizontalVelocityTimer FixedUpdate: " + reducingHorizontalVelocityTimer.ToString());
                parkourCounter.Log("t FixedUpdate: " + t.ToString());
                parkourCounter.Log("Velocity FixedUpdate: " + rb.velocity.ToString());
                parkourCounter.Log("newVelocity FixedUpdate: " + newVelocity.ToString());
                rb.velocity = newVelocity;
                reducingHorizontalVelocityTimer -= Time.fixedDeltaTime;
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
        }
        
        if (flyMethode == FlyMethode.HMD && minHMDLocalHeight != 0.0f && maxHMDLocalHeight != 0.0f) // && rightTriggerValue > 0.75f
        {
            // Fallback, if the raycast does not hit anything, for whatever weird reason
            float tempValueY = hmd.transform.position.y + 2f;

            // parkourCounter.Log("hmd.transform.position.y: " + hmd.transform.position.y.ToString() + "");
            // parkourCounter.Log("hmd.transform.localPosition.y: " + hmd.transform.localPosition.y.ToString() + "");
            // get the height of the first object under the player, which should be the floor
            // transform.down does not exist, so invert the up vector
            RaycastHit floor;
            float floorY = 0f;
            if (Physics.Raycast(player.transform.position, -player.transform.up, out floor, Mathf.Infinity, layerTerrain))
            {
                tempValueY = floor.point.y + 0.65f;
                floorY = floor.point.y;
            }


            // Move player ridigbody to the hmd y position but leave the rest unchanged
            // layer.GetComponent<Rigidbody>().MovePosition(new Vector3(player.transform.position.x, tempValueY, player.transform.position.z));

            // Treat minHMDLocalHeight as 0 and maxHMDLocalHeight as 1
            // Interpolate the currrent hmd.transform.localPosition.y to the range of 0 to 1
            // Calculate a force. At the value of 0.5 it should apply Physics.Gravity so that the height stays the same, for lower then 0.5 it should become less until the applied force is zero and for higher than 0.5 it should become more until the applied force is the double of Physics.Gravity
            // Interpolate the current HMD local position to a range of 0 to 1

            // float normalizedHeight = Mathf.InverseLerp(minHMDLocalHeight, maxHMDLocalHeight, hmd.transform.localPosition.y);
            float relHeight = rightHand.transform.position.y - player.transform.position.y;
            float normalizedHeight = Mathf.InverseLerp(minHMDLocalHeight, maxHMDLocalHeight, relHeight);

            // Log noramlizedHeight
            parkourCounter.Log("normalizedHeight: " + normalizedHeight.ToString());

            Vector3 force;
            // Make it more likeley to being able to hold the height
            if (normalizedHeight > 0.45 && normalizedHeight < 0.56)
            {
                normalizedHeight = 0.5f;
            }


            // force = Vector3.Lerp(Vector3.zero, -1.25f * Physics.gravity, normalizedHeight);
            if (normalizedHeight < 0.5f)
            {
                // If normalizedHeight is less than 0.5, interpolate between 0 and -Physics.gravity
                force = Vector3.Lerp(Vector3.zero, -Physics.gravity, normalizedHeight * 2);
            }
            else
            {
                // If normalizedHeight is greater than or equal to 0.5, interpolate between -Physics.gravity and -2 * Physics.gravity
                force = Vector3.Lerp(-Physics.gravity, -1.45f * Physics.gravity, (normalizedHeight - 0.5f) * 2);
            }

            // Vector3 forceBuildUpFlyVector = new Vector3(0, 10f * Time.deltaTime, 0);
            // force = Vector3.Lerp(Vector3.zero, forceBuildUpFlyVector, normalizedHeight);

            force = new Vector3(force.x, force.y * Time.fixedDeltaTime, force.z);
            // force.y = force.y - (Physics.gravity.y * player.GetComponent<Rigidbody>().mass);
            selectionTaskMeasure.scoreText.text = "  " + normalizedHeight.ToString() + " - " + force.ToString();

            // Log force
            parkourCounter.Log("force: " + force.ToString());

            
            if (normalizedHeight == 0.5f)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
            else if(force.y > 0f)
            {
                // Apply the force to the Rigidbody
                // player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Acceleration);
                rb.AddForce(force, ForceMode.Impulse);
            }

            // height = Vector3.Lerp(3f, 10f, normalizedHeight);
            float height = 7f * normalizedHeight + floorY;
            parkourCounter.Log("height: " + height.ToString());
            // player.transform.position = new Vector3(player.transform.position.x, height, player.transform.position.z);
        }
    }

    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                player.transform.position = parkourCounter.currentRespawnPos;
            }
        }
        if (OVRInput.Get(OVRInput.Button.One))
        {
            // minHMDLocalHeight = hmd.transform.localPosition.y;
            minHMDLocalHeight = rightHand.transform.position.y - player.transform.position.y;
        }
        if (OVRInput.Get(OVRInput.Button.Three))
        {
            // maxHMDLocalHeight = hmd.transform.localPosition.y;
            maxHMDLocalHeight = rightHand.transform.position.y - player.transform.position.y;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController); 
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);

        if (preventMovement == true) { return; }

        /*if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
        {
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos = (OVRInput.GetLocalControllerPosition(leftController) + OVRInput.GetLocalControllerPosition(rightController)) / 2;
            }
            offset = hmd.transform.forward.normalized *
                    ((OVRInput.GetLocalControllerPosition(leftController) - startPos) +
                     (OVRInput.GetLocalControllerPosition(rightController) - startPos)).magnitude;
            Debug.DrawRay(startPos, offset, Color.red, 0.2f);
        }
        else if (leftTriggerValue > 0.95f && rightTriggerValue < 0.95f)
        {
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos = OVRInput.GetLocalControllerPosition(leftController);
            }
            offset = hmd.transform.forward.normalized *
                     (OVRInput.GetLocalControllerPosition(leftController) - startPos).magnitude;
            Debug.DrawRay(startPos, offset, Color.red, 0.2f);
        }
        else if (leftTriggerValue < 0.95f && rightTriggerValue > 0.95f)
        {
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos = OVRInput.GetLocalControllerPosition(rightController);
            }
           offset = hmd.transform.forward.normalized *
                    (OVRInput.GetLocalControllerPosition(rightController) - startPos).magnitude;
            Debug.DrawRay(startPos, offset, Color.red, 0.2f);
        }

        player.transform.position = player.transform.position + (offset) * translationGain;*/

        offset = Vector3.zero;

        if (leftTriggerValue > 0.95f)
        {
            if (!isLeftTriggerDown)
            {
                isLeftTriggerDown = true;
                forceBuildUp = 0.14f;
            }
            else
            {
                // forceBuildUp += 0.01f * Time.deltaTime;
                if(forceBuildUp > maxForce)
                {
                    forceBuildUp = maxForce;
                }
            }
            parkourCounter.Log("hmd.transform.forward.normalized: " + hmd.transform.forward.normalized.ToString());
            tmp = Vector3.Scale(hmd.transform.forward.normalized,flatVector);
            // parkourCounter.Log(tmp.ToString());
            // tmp = Vector.Multiply(hmd.transform.forward.normalized * flatVector);

            // TODO: FIXME: Temporary disabled, trying moving to FixedUpdate()
            // offset += tmp * forceBuildUp;

        }
        else
        {
            if(isLeftTriggerDown)
            {
                reducingHorizontalVelocityTimer = reducingHorizontalVelocityTimeMax;
            }
            isLeftTriggerDown = false;
        }

        if(flyMethode == FlyMethode.Controller)
        {
            // parkourCounter.Log("rightTriggerValue:" + rightTriggerValue);
            // TODO: FIXME: is my right controller damaged? often times it seems to only reach a trigger value of up to ~80 while pressing it fully
            if (rightTriggerValue > 0.75f)
            {
                if (!isRightTriggerDown)
                {
                    isRightTriggerDown = true;
                    forceBuildUpFly = 0.2f;
                }
                else
                {
                    // forceBuildUpFly += 0.01f * Time.deltaTime;
                    if (forceBuildUpFly > maxForceFly)
                    {
                        forceBuildUpFly = maxForceFly;
                    }
                }

                // get the height of the headset


                // parkourCounter.Log((upVector * forceBuildUpFly).ToString());
                parkourCounter.Log("up vector: " + (player.transform.up.normalized * forceBuildUpFly).ToString());
                // offset += upVector * forceBuildUpFly;
                /*parkourCounter.Log("hmd.tansform.up:");
                parkourCounter.Log(hmd.transform.up.normalized.ToString());
                parkourCounter.Log("player.transform.up.normalized:");
                parkourCounter.Log(player.transform.up.normalized.ToString());*/
                offset += player.transform.up.normalized * forceBuildUpFly;
                // offset += hmd.transform.up.normalized * forceBuildUpFly;
            }
            else
            {
                isRightTriggerDown = false;
            }
        }
        /*if (leftTriggerValue > 0.95f && rightTriggerValue < 0.95f)
        {
            if (isIndexTriggerDown)
            {
                isIndexTriggerDown = false;
            }
        }*/

        // parkourCounter.Log("hmd forward:");
        // parkourCounter.Log(hmd.transform.forward.normalized.ToString());
        if(offset != Vector3.zero)
        {
            parkourCounter.Log("offset not zero: " + offset.ToString());
           
        }

        // Really hard to change directions
        player.GetComponent<Rigidbody>().AddForce(offset, ForceMode.Impulse);

        // Would need time to figure out meanigfull values
        // player.GetComponent<Rigidbody>().AddForce(offset, ForceMode.Acceleration);

        if(rightTriggerValue > 0.75f)
        {
            parkourCounter.Log("triggered hmd.transform.position.y: " + hmd.transform.position.y.ToString() + "");
            parkourCounter.Log("triggered hmd.transform.localPosition.y: " + hmd.transform.localPosition.y.ToString() + "");
            parkourCounter.Log("triggered rightHand.transform.localPosition.y: " + rightHand.transform.localPosition.y.ToString() + "");
            parkourCounter.Log("triggered rightHand.transform.position.y: " + rightHand.transform.position.y.ToString() + "");
            parkourCounter.Log("triggered player.transform.position.y: " + player.transform.position.y.ToString() + "");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        // These are for the game mechanism.
        if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("objectInteractionTask"))
        {
            selectionTaskMeasure.isTaskStart = true;
            selectionTaskMeasure.scoreText.text = "";
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            // rotation: facing the user's entering direction

            // float tempValueY = other.transform.position.y > 0 ? 12 : 0;

            // Fallback, if the raycast does not hit anything, for whatever weird reason
            float tempValueY = player.transform.position.y;

            // get the height of the first object under the player, which should be the floor
            // transform.down does not exist, so invert the up vector
            RaycastHit floor;
            if (Physics.Raycast(player.transform.position, -player.transform.up, out floor, Mathf.Infinity, layerTerrain))
            {
                tempValueY = floor.point.y + 0.65f;
            }
            
            // paranoia check, if we hit something unrealistic height for whatever reason
            if(tempValueY > (player.transform.position.y + 2f))
            {
                tempValueY = player.transform.position.y;
            }

            // Vector3 tmpTarget = new Vector3(hmd.transform.position.x, tempValueY, hmd.transform.position.z);
            Vector3 tmpTarget = new Vector3(player.transform.position.x, tempValueY, player.transform.position.z);
            selectionTaskMeasure.taskUI.transform.position = new Vector3(selectionTaskMeasure.taskUI.transform.position.x, tempValueY, selectionTaskMeasure.taskUI.transform.position.z);
            selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
            selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
            parkourCounter.Log("transform.position: " + other.transform.position.ToString());
            parkourCounter.Log("hmd.transform.position: " + hmd.transform.position.ToString());
            parkourCounter.Log("selectionTaskMeasure.taskUI.transform.position: " + selectionTaskMeasure.taskUI.transform.position.ToString());
            selectionTaskMeasure.portalEnter.SetActive(true);
            selectionTaskMeasure.taskStartPanel.SetActive(true);
            parkourCounter.SetTextForCurrentBlockade(selectionTaskMeasure.tasksNum.ToString());
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        // These are for the game mechanism.
    }

    public void IntroductionScene()
    {
        startingAreaRadio.GetComponent<AudioSource>().Play();
        StartCoroutine(IntroductionScenePart2());
    }

    IEnumerator IntroductionScenePart2()
    {
        yield return new WaitForSeconds(15);
        startingAreaExplosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(IntroductionSceneEnd());
    }

    IEnumerator IntroductionSceneEnd()
    {
        yield return new WaitForSeconds(2);
        startingArea.SetActive(false);
    }
}
