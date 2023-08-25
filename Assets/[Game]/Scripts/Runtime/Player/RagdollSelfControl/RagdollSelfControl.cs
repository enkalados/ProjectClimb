using UnityEngine;

public class RagdollSelfControl : MonoBehaviour
{
    public GameObject[] PlayerParts;
    public ConfigurableJoint[] JointParts;
    Vector3 COM;
    [SerializeField] float touchForce, timeStep, legsHeight, fallFactor;
    float step_R_Time, step_L_Time;
    bool stepR, stepL, walkF, walkB, falling, fall;
    bool flag_Leg_R, flag_Leg_L;
    Quaternion startLegR1, startLegR2, startLegL1, startLegL2;
    JointDrive spring0, spring150, spring300, spring320;

    private void Awake()
    {
        Physics.IgnoreCollision(PlayerParts[2].GetComponent<Collider>(), PlayerParts[4].GetComponent<Collider>(), true);
        Physics.IgnoreCollision(PlayerParts[3].GetComponent<Collider>(), PlayerParts[7].GetComponent<Collider>(), true);
        startLegR1 = PlayerParts[4].GetComponent<ConfigurableJoint>().targetRotation;
        startLegR2 = PlayerParts[5].GetComponent<ConfigurableJoint>().targetRotation;
        startLegL1 = PlayerParts[7].GetComponent<ConfigurableJoint>().targetRotation;
        startLegL2 = PlayerParts[8].GetComponent<ConfigurableJoint>().targetRotation;

        spring0 = new JointDrive();
        spring0.positionSpring = 0;
        spring0.positionDamper = 0;
        spring0.maximumForce = Mathf.Infinity;

        spring150 = new JointDrive();
        spring150.positionSpring = 150;
        spring150.positionDamper = 0;
        spring150.maximumForce = Mathf.Infinity;

        spring300 = new JointDrive();
        spring300.positionSpring = 300;
        spring300.positionDamper = 100;
        spring300.maximumForce = Mathf.Infinity;

        spring320 = new JointDrive();
        spring320.positionSpring = 320;
        spring320.positionDamper = 0;
        spring320.maximumForce = Mathf.Infinity;
    }

    private void Update()
    {
        //PlayerParts[12].transform.position = Vector3.Lerp(PlayerParts[12].transform.position, PlayerParts[2].transform.position, 2 * Time.unscaledDeltaTime); //cam

        #region Input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerParts[0].GetComponent<Rigidbody>().AddForce(Vector3.forward * touchForce, ForceMode.Impulse);
        }
        #endregion

        Calculate_COM();

        PlayerParts[10].transform.position = COM;

        Balance();

        //PlayerParts[11].transform.LookAt(PlayerParts[10].transform.position);//cam

        if (!walkF && !walkB)
        {
            stepR = false;
            stepL = false;
            step_R_Time = 0;
            step_L_Time = 0;
            flag_Leg_R = false;
            flag_Leg_L = false;
            JointParts[0].targetRotation = Quaternion.Lerp(JointParts[0].targetRotation, new Quaternion(-0.1f, JointParts[0].targetRotation.y, JointParts[0].targetRotation.z, JointParts[0].targetRotation.w), 6 * Time.fixedDeltaTime);
        }
    }

    private void FixedUpdate()
    {
        LegsMoving();
    }

    void Balance()
    {
        if (PlayerParts[10].transform.position.z < PlayerParts[6].transform.position.z && PlayerParts[10].transform.position.z < PlayerParts[9].transform.position.z)
        {
            walkB = true;
            JointParts[0].targetRotation = Quaternion.Lerp(JointParts[0].targetRotation, new Quaternion(-0.1f, JointParts[0].targetRotation.y, JointParts[0].targetRotation.z, JointParts[0].targetRotation.w), 6 * Time.fixedDeltaTime);
        }
        else
        {
            walkB = false;
        }

        if (PlayerParts[10].transform.position.z > PlayerParts[6].transform.position.z && PlayerParts[10].transform.position.z > PlayerParts[9].transform.position.z)
        {
            walkF = true;
            JointParts[0].targetRotation = Quaternion.Lerp(JointParts[0].targetRotation, new Quaternion(0, JointParts[0].targetRotation.y, JointParts[0].targetRotation.z, JointParts[0].targetRotation.w), 6 * Time.fixedDeltaTime);
        }
        else
        {
            walkF = false;
        }

        if (PlayerParts[10].transform.position.z > PlayerParts[6].transform.position.z + fallFactor &&
           PlayerParts[10].transform.position.z > PlayerParts[9].transform.position.z + fallFactor ||
           PlayerParts[10].transform.position.z < PlayerParts[6].transform.position.z - (fallFactor + 0.2f) &&
           PlayerParts[10].transform.position.z < PlayerParts[9].transform.position.z - (fallFactor + 0.2f))
        {
            falling = true;
        }
        else
        {
            falling = false;
        }

        if (falling)
        {
            JointParts[1].angularXDrive = spring0;
            JointParts[1].angularYZDrive = spring0;
            legsHeight = 5;
        }
        else
        {
            JointParts[1].angularXDrive = spring300;
            JointParts[1].angularYZDrive = spring300;
            legsHeight = 1;
            JointParts[2].targetRotation = Quaternion.Lerp(JointParts[2].targetRotation, new Quaternion(0, JointParts[2].targetRotation.y, JointParts[2].targetRotation.z, JointParts[2].targetRotation.w), 6 * Time.fixedDeltaTime);
            JointParts[3].targetRotation = Quaternion.Lerp(JointParts[3].targetRotation, new Quaternion(0, JointParts[3].targetRotation.y, JointParts[3].targetRotation.z, JointParts[3].targetRotation.w), 6 * Time.fixedDeltaTime);
            JointParts[2].angularXDrive = spring0;
            JointParts[2].angularYZDrive = spring150;
            JointParts[3].angularXDrive = spring0;
            JointParts[3].angularYZDrive = spring150;
        }

        if (PlayerParts[0].transform.position.y - 0.1f <= PlayerParts[1].transform.position.y)
        {
            fall = true;
        }
        else
        {
            fall = false;
        }

        if (fall)
        {
            JointParts[1].angularXDrive = spring0;
            JointParts[1].angularYZDrive = spring0;
            StandUping();
        }
    }

    void LegsMoving()
    {
        if (walkF)
        {
            if (PlayerParts[6].transform.position.z < PlayerParts[9].transform.position.z && !stepL && !flag_Leg_R)
            {
                stepR = true;
                flag_Leg_R = true;
                flag_Leg_L = true;
            }
            if (PlayerParts[6].transform.position.z > PlayerParts[9].transform.position.z && !stepR && !flag_Leg_L)
            {
                stepL = true;
                flag_Leg_L = true;
                flag_Leg_R = true;
            }
        }

        if (walkB)
        {
            if (PlayerParts[6].transform.position.z > PlayerParts[9].transform.position.z && !stepL && !flag_Leg_R)
            {
                stepR = true;
                flag_Leg_R = true;
                flag_Leg_L = true;
            }
            if (PlayerParts[6].transform.position.z < PlayerParts[9].transform.position.z && !stepR && !flag_Leg_L)
            {
                stepL = true;
                flag_Leg_L = true;
                flag_Leg_R = true;
            }
        }

        if (stepR)
        {
            step_R_Time += Time.fixedDeltaTime;

            if (walkF)
            {
                JointParts[4].targetRotation = new Quaternion(JointParts[4].targetRotation.x + 0.07f * legsHeight, JointParts[4].targetRotation.y, JointParts[4].targetRotation.z, JointParts[4].targetRotation.w);
                JointParts[5].targetRotation = new Quaternion(JointParts[5].targetRotation.x - 0.04f * legsHeight * 2, JointParts[5].targetRotation.y, JointParts[5].targetRotation.z, JointParts[5].targetRotation.w);

                JointParts[7].targetRotation = new Quaternion(JointParts[7].targetRotation.x - 0.02f * legsHeight / 2, JointParts[7].targetRotation.y, JointParts[7].targetRotation.z, JointParts[7].targetRotation.w);
            }

            if (walkB)
            {
                JointParts[4].targetRotation = new Quaternion(JointParts[4].targetRotation.x - 0.00f * legsHeight, JointParts[4].targetRotation.y, JointParts[4].targetRotation.z, JointParts[4].targetRotation.w);
                JointParts[5].targetRotation = new Quaternion(JointParts[5].targetRotation.x - 0.06f * legsHeight * 2, JointParts[5].targetRotation.y, JointParts[5].targetRotation.z, JointParts[5].targetRotation.w);

                JointParts[7].targetRotation = new Quaternion(JointParts[7].targetRotation.x + 0.02f * legsHeight / 2, JointParts[7].targetRotation.y, JointParts[7].targetRotation.z, JointParts[7].targetRotation.w);
            }

            if (step_R_Time > timeStep)
            {
                step_R_Time = 0;
                stepR = false;

                if (walkB || walkF)
                {
                    stepL = true;
                }
            }
        }
        else
        {
            JointParts[4].targetRotation = Quaternion.Lerp(JointParts[4].targetRotation, startLegR1, (8f) * Time.fixedDeltaTime);
            JointParts[5].targetRotation = Quaternion.Lerp(JointParts[5].targetRotation, startLegR2, (17f) * Time.fixedDeltaTime);
        }

        if (stepL)
        {
            step_L_Time += Time.fixedDeltaTime;

            if (walkF)
            {
                JointParts[7].targetRotation = new Quaternion(JointParts[7].targetRotation.x + 0.07f * legsHeight, JointParts[7].targetRotation.y, JointParts[7].targetRotation.z, JointParts[7].targetRotation.w);
                JointParts[8].targetRotation = new Quaternion(JointParts[8].targetRotation.x - 0.04f * legsHeight * 2, JointParts[8].targetRotation.y, JointParts[8].targetRotation.z, JointParts[8].targetRotation.w);

                JointParts[4].targetRotation = new Quaternion(JointParts[4].targetRotation.x - 0.02f * legsHeight / 2, JointParts[4].targetRotation.y, JointParts[4].targetRotation.z, JointParts[4].targetRotation.w);
            }

            if (walkB)
            {
                JointParts[7].targetRotation = new Quaternion(JointParts[7].targetRotation.x - 0.00f * legsHeight, JointParts[7].targetRotation.y, JointParts[7].targetRotation.z, JointParts[7].targetRotation.w);
                JointParts[8].targetRotation = new Quaternion(JointParts[8].targetRotation.x - 0.06f * legsHeight * 2, JointParts[8].targetRotation.y, JointParts[8].targetRotation.z, JointParts[8].targetRotation.w);

                JointParts[4].targetRotation = new Quaternion(JointParts[4].targetRotation.x + 0.02f * legsHeight / 2, JointParts[4].targetRotation.y, JointParts[4].targetRotation.z, JointParts[4].targetRotation.w);
            }

            if (step_L_Time > timeStep)
            {
                step_L_Time = 0;
                stepL = false;

                if (walkB || walkF)
                {
                    stepR = true;
                }
            }
        }
        else
        {
            JointParts[7].targetRotation = Quaternion.Lerp(JointParts[7].targetRotation, startLegL1, (8) * Time.fixedDeltaTime);
            JointParts[8].targetRotation = Quaternion.Lerp(JointParts[8].targetRotation, startLegL2, (17) * Time.fixedDeltaTime);
        }
    }

    void StandUping()
    {
        if (walkF)
        {
            JointParts[2].angularXDrive = spring320;
            JointParts[2].angularYZDrive = spring320;
            JointParts[3].angularXDrive = spring320;
            JointParts[3].angularYZDrive = spring320;
            JointParts[0].targetRotation = Quaternion.Lerp(JointParts[0].targetRotation, new Quaternion(-0.1f, JointParts[0].targetRotation.y,
                JointParts[0].targetRotation.z, JointParts[0].targetRotation.w), 6 * Time.fixedDeltaTime);

            if (JointParts[2].targetRotation.x < 1.7f)
            {
                JointParts[2].targetRotation = new Quaternion(JointParts[2].targetRotation.x + 0.07f, JointParts[2].targetRotation.y,
                    JointParts[2].targetRotation.z, JointParts[2].targetRotation.w);
            }

            if (JointParts[3].targetRotation.x < 1.7f)
            {
                JointParts[3].targetRotation = new Quaternion(JointParts[3].targetRotation.x + 0.07f, JointParts[3].targetRotation.y,
                    JointParts[3].targetRotation.z, JointParts[3].targetRotation.w);
            }
        }

        if (walkB)
        {
            JointParts[2].angularXDrive = spring320;
            JointParts[2].angularYZDrive = spring320;
            JointParts[3].angularXDrive = spring320;
            JointParts[3].angularYZDrive = spring320;

            if (JointParts[2].targetRotation.x > -1.7f)
            {
                JointParts[2].targetRotation = new Quaternion(JointParts[2].targetRotation.x - 0.09f, JointParts[2].targetRotation.y,
                    JointParts[2].targetRotation.z, JointParts[2].targetRotation.w);
            }

            if (JointParts[3].targetRotation.x > -1.7f)
            {
                JointParts[3].targetRotation = new Quaternion(JointParts[3].targetRotation.x - 0.09f, JointParts[3].targetRotation.y,
                    JointParts[3].targetRotation.z, JointParts[3].targetRotation.w);
            }
        }
    }

    void Calculate_COM()
    {
        COM = (JointParts[0].GetComponent<Rigidbody>().mass * JointParts[0].transform.position +
            JointParts[1].GetComponent<Rigidbody>().mass * JointParts[1].transform.position +
            JointParts[2].GetComponent<Rigidbody>().mass * JointParts[2].transform.position +
            JointParts[3].GetComponent<Rigidbody>().mass * JointParts[3].transform.position +
            JointParts[4].GetComponent<Rigidbody>().mass * JointParts[4].transform.position +
            JointParts[5].GetComponent<Rigidbody>().mass * JointParts[5].transform.position +
            JointParts[6].GetComponent<Rigidbody>().mass * JointParts[6].transform.position +
            JointParts[7].GetComponent<Rigidbody>().mass * JointParts[7].transform.position +
            JointParts[8].GetComponent<Rigidbody>().mass * JointParts[8].transform.position +
            JointParts[9].GetComponent<Rigidbody>().mass * JointParts[9].transform.position) /
            (JointParts[0].GetComponent<Rigidbody>().mass + JointParts[1].GetComponent<Rigidbody>().mass +
            JointParts[2].GetComponent<Rigidbody>().mass + JointParts[3].GetComponent<Rigidbody>().mass +
            JointParts[4].GetComponent<Rigidbody>().mass + JointParts[5].GetComponent<Rigidbody>().mass +
            JointParts[6].GetComponent<Rigidbody>().mass + JointParts[7].GetComponent<Rigidbody>().mass +
            JointParts[8].GetComponent<Rigidbody>().mass + JointParts[9].GetComponent<Rigidbody>().mass);
    }
    public void ForceToBody(Vector3 dir, float force)
    {
        //PlayerParts[0].GetComponent<Rigidbody>().AddForce(Vector3.back * touchForce, ForceMode.Impulse);
        PlayerParts[0].GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
    }
}
