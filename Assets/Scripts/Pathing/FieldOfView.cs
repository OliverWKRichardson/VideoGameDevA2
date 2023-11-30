using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // max distance a target can be seen from
    public float viewRadius;
    // angle of the cone of vision
    [Range(0,360)] public float viewAngle;

    // layer mask of targets
    public LayerMask targetMask;
    // layermask of objects that can block sight (dont include self or target)
    public LayerMask ignoreMask;

    // list of visible targets
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        // 2nd arg is delay for refreshing the current visiblity list
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while(true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            if(visibleTargets.Count > 0)
            {
                GetComponent<Pathing>().SeeSomething(true, visibleTargets);
            }
            else
            {
                GetComponent<Pathing>().SeeSomething(false, visibleTargets);
            }
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for(int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float disToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, disToTarget, ignoreMask)){
                    // can see target
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
