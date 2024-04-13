using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player follow;
    public Vector3 offset;
    [Range(-1f, float.MinValue)]
    public float leftThreshold;
    [Range(1f, float.MaxValue)]
    public float rightThreshold;
    [Range(-1f, float.MinValue)]
    public float leftStop;
    [Range(1f, float.MaxValue)]
    public float rightStop;

    private Vector3 v = Vector3.zero;

    private void OnEnable()
    {
        PlayerSummons.OnSummonChange += ChangeFollow;
    }

    private void ChangeFollow(Player newFollow)
    {
        follow = newFollow;
    }

    public void LateUpdate()
    {
        var rightOffset = offset + (Vector3.right * rightStop);
        var leftOffset = offset + (Vector3.right * leftStop);
        Debug.Log(follow.transform.position.x);
        if (follow.transform.position.x < leftThreshold)
            transform.position = Vector3.SmoothDamp(transform.position, leftOffset, ref v, 1);
        else if(follow.transform.position.x > rightThreshold)
            transform.position = Vector3.SmoothDamp(transform.position, rightOffset, ref v, 1);
        else
            transform.position = Vector3.SmoothDamp(transform.position, offset, ref v, 1);

    }

    private void OnDisable()
    {
        PlayerSummons.OnSummonChange -= ChangeFollow;
    }
}
