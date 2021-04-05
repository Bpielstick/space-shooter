using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != transform.parent.gameObject && other.gameObject.tag != "Player" && other.gameObject.tag != "Shield" 
            && other.gameObject.tag != "Laser" && other.gameObject.tag != "Missile" && other.gameObject.tag != "EnemyLaser")
        { 
        SendMessageUpwards("AvoidCollision", other.transform.position);
        Debug.Log("collided with " + other.tag);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        SendMessageUpwards("ClearCollision");
        
    }
}
