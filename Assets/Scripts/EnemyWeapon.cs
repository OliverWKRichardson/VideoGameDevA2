using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public LineRenderer beam;
    public Transform barrelEnd;

    public ParticleSystem hitParticles;
    public ParticleSystem emissionParticles;

    public float maxLength;
    public bool fire;
    public float damage;

    private void Activate()
    {
        beam.enabled = true;
        hitParticles.Play();
        emissionParticles.Play();
    }

    private void Deactivate()
    {
        beam.enabled = false;
        beam.SetPosition(0, barrelEnd.position);
        beam.SetPosition(1, barrelEnd.position);
        hitParticles.Stop();
        emissionParticles.Stop();
    }

    void Awake()
    {
        beam.enabled = false;
        fire = false;
    }

    void Update()
    {
        if(fire)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    void FixedUpdate()
    {
        if(beam.enabled)
        {
            Ray ray = new Ray(barrelEnd.position, barrelEnd.forward);
            bool cast = Physics.Raycast(ray, out RaycastHit hit, maxLength);
            Vector3 hitPosition = cast ? hit.point : barrelEnd.position + barrelEnd.forward * maxLength;

            beam.SetPosition(0, barrelEnd.position);
            beam.SetPosition(1, hitPosition);

            hitParticles.transform.position = hitPosition;

            if(cast && hit.collider.TryGetComponent(out HealthManager healthManager))
            {
                healthManager.DamageBy(damage * Time.deltaTime);
            }
        }
    }
}

