using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask whatIsProp;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float lifeTime = 10f;
    public float explosionRadious = 20f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadious,whatIsProp); 
        
        for(int i=0; i<colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            targetRigidbody.AddExplosionForce(explosionForce,transform.position,explosionRadious);

            Prop targetProp = colliders[i].GetComponent<Prop>();
            float damage = CalculateDamage(colliders[i].transform.position); //받을 데미지 반환
            targetProp.TakeDamage(damage); //해당 프롭에 데미지 입히기
        }

        explosionParticle.transform.parent = null; //파티클 자식에서 떼어냄
        explosionParticle.Play();
        explosionAudio.Play();

        GameManager.instance.OnBallDestroy();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition) //폭발 중심에 가까우면 데미지 큼
    {
        Vector3 explosionToTarget = targetPosition - transform.position;
        float distance = explosionToTarget.magnitude;
        float edgeToCenterDistance = explosionRadious - distance;
        float percentage =edgeToCenterDistance/ explosionRadious;
        float damage = maxDamage * percentage;

        damage = Mathf.Max(0, damage); //데미지가 -일 때를 대비해서

        return damage;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnBallDestroy();
    }
}
