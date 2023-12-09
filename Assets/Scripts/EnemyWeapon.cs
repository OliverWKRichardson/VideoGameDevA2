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
    private AudioSource sound;
    private bool playing;

    private void Activate()
    {
        if(!playing)
        {
            beam.enabled = true;
            sound.Play();
            hitParticles.Play();
            emissionParticles.Play();
            playing = true;
        }
    }

    private void Deactivate()
    {
        if(playing)
        {
            beam.enabled = false;
            beam.SetPosition(0, barrelEnd.position);
            beam.SetPosition(1, barrelEnd.position);
            hitParticles.Stop();
            emissionParticles.Stop();
            sound.Stop();
            playing = false;
        } 
    }

    void Awake()
    {
        beam.enabled = false;
        fire = false;
        sound = GetComponent<AudioSource>();
        playing = false;
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

