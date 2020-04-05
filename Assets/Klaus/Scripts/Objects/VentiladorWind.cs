using UnityEngine;
using System.Collections;

public class VentiladorWind : MonoBehaviour
{
    public float MaxDistance = 100.0f;
    public bool useSide = false;
    public float SideAmount = 0.05f;

    #region SideVar:

    RaycastHit2D[] result = new RaycastHit2D[5];
    RaycastHit2D[] resultL = new RaycastHit2D[5];
    RaycastHit2D[] resultR = new RaycastHit2D[5];

    #endregion

    /// <summary>
    /// What layers are ground.
    /// </summary>
    public LayerMask whatIsGround;

    static float FactorSizeY = 1;

    public ParticleSystem particles;
    public float ConvertionFact = 0.09f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ManagerPause.Pause)
        {
            bool canCount = Physics2D.RaycastNonAlloc(transform.position, transform.up, result, MaxDistance, whatIsGround) > 0;
            float dist = canCount ? result [0].distance : MaxDistance;
            if (useSide)
            {
                int count2 = Physics2D.RaycastNonAlloc(transform.position + transform.right * SideAmount * -1, transform.up, resultL, MaxDistance, whatIsGround);
                int count3 = Physics2D.RaycastNonAlloc(transform.position + transform.right * SideAmount, transform.up, resultR, MaxDistance, whatIsGround);
           
         
                if (count2 > 0)
                {
                    dist = dist < resultL [0].distance ? dist : resultL [0].distance;
                    canCount = true;
                    Debug.DrawLine(transform.position + transform.right * SideAmount * -1, transform.position + transform.right * SideAmount * -1 + transform.up * dist);

                } 
                if (count3 > 0)
                {
                    dist = dist < resultR [0].distance ? dist : resultR [0].distance;
                    canCount = true;
                    Debug.DrawLine(transform.position + transform.right * SideAmount, transform.position + transform.right * SideAmount + transform.up * dist);


                }

            }
            Debug.DrawLine(transform.position, transform.position + transform.up * dist);
            
            if (transform.localScale.y != dist)
            {
                particles.startLifetime = dist * FactorSizeY * ConvertionFact;
                transform.localScale = new Vector3(transform.localScale.x, dist * FactorSizeY, transform.localScale.z); 
            }
  
        }
    }
}
