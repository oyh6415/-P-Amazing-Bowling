using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public enum State
    {
        Idle,Ready,Tracking
    }
    
    private State state
    {
        set
        {
            switch (value)
            {
                case State.Idle:
                    targetZoomSize = roundReadZoomSize;
                    break;
                case State.Ready:
                    targetZoomSize = readyShotZoomSize;
                    break;
                case State.Tracking:
                    targetZoomSize = trackingZoomSize;
                    break;
            }
        }
    }

    private Transform target;

    public float smoothTime = 0.2f; //지연 시간
    private Vector3 lastMovingVelocity;
    private Vector3 movingVelocity;
    private Vector3 targetPosition;

    private Camera cam;
    private float targetZoomSize = 5f;

    private const float roundReadZoomSize = 14.5f;
    private const float readyShotZoomSize = 5f;
    private const float trackingZoomSize = 10f;

    private float lastZoomSpeed;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        state = State.Idle;
    }

    private void Move()
    {
        targetPosition = target.transform.position;
        Vector3 smoothPostion = Vector3.SmoothDamp(transform.position, targetPosition,
            ref lastMovingVelocity, smoothTime);

        transform.position = smoothPostion;
    }

    private void Zoom()
    {
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize,
            ref lastZoomSpeed, smoothTime);

        cam.orthographicSize = smoothZoomSize;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Move();
            Zoom();
        }
    }

    public void ResetState()
    {
        state = State.Idle;
    }

    public void SetTarget(Transform newTarget, State newState)
    {
        target = newTarget;
        state = newState;
    }
}
