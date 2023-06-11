using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiFollow : MonoBehaviour
{
    public Transform ai;
    public Animation aiAnim;
    public float curDis;
  
    public void Jump()
    {

    }
    public void LeftDodge()
    {

    }
    public void RightDodge()
    {

    }
    public void Caught()
    {

    }

    IEnumerator PlayAnim(string anim)
    {
        yield return new WaitForSeconds(curDis / 5f);
        aiAnim.Play(anim);
    }
    public void FollowAI(Vector3 pos, float speed)
    {
        Vector3 position = pos - Vector3.forward * curDis;
        ai.position = Vector3.Lerp(ai.position, position, Time.deltaTime * speed / curDis);


    }
}
