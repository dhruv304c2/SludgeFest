using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerTraversalScript : MonoBehaviour
{
    [SerializeField] Transform GroundPoint;
    [SerializeField] Transform WallPoint;
    [SerializeField] string CornerSide;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>())
        {
            var m_playerMovement = collision.GetComponent<PlayerMovement>();
            if(m_playerMovement.Swimming && m_playerMovement.WallRunning)
            {
                if (m_playerMovement.IsWallRight())
                {
                    if (Input.GetAxis("Horizontal") > 0f)
                    {
                        m_playerMovement.transform.position = Vector2.MoveTowards(m_playerMovement.transform.position,GroundPoint.position,1f);
                        m_playerMovement.IsTouchingGround = true;
                        m_playerMovement.CanDissolve = true;
                        m_playerMovement.WallRunning = false;
                    }
                }
                
                else if (m_playerMovement.IsWallLeft())
                {
                    if (Input.GetAxis("Horizontal") < 0f)
                    {
                        m_playerMovement.transform.position = GroundPoint.position;
                        m_playerMovement.IsTouchingGround = true;
                        m_playerMovement.CanDissolve = true;
                        m_playerMovement.WallRunning = false;
                    }
                }
            }
            else if (m_playerMovement.Swimming && !m_playerMovement.WallRunning)
            {
                if (CornerSide == "LeftFacing")
                {
                    if (Input.GetAxis("Horizontal") < 0f)
                    {
                        m_playerMovement.transform.position = WallPoint.position;
                        m_playerMovement.WallRunning = true;
                    }
                }

                else if (CornerSide == "RightFacing")
                {
                    if (Input.GetAxis("Horizontal") > 0f)
                    {
                        m_playerMovement.transform.position = WallPoint.position;
                        m_playerMovement.WallRunning = true;
                    }
                }
            }
        }
    }
}
