using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPool : MonoBehaviour
{
    [SerializeField] private ParticleSystem _prefab;
    private Queue<ParticleSystem> _particlesPool;

    void Awake()
    {
        _particlesPool = new Queue<ParticleSystem>();
    }

    public void ActivateParticle(Vector3 position)
    {
        ParticleSystem particle = GetParticleFromPool(); 

        particle.transform.position = position;
        particle.Play();

        StartCoroutine(ReturnParticleToPool(particle, particle.main.duration));
    }

    public void ActivateParticle(Vector3 position , Color color)
    {
        ParticleSystem particle = GetParticleFromPool();

        particle.transform.position = position;
        var mainParticle = particle.main;
        mainParticle.startColor = color;
        particle.Play();

        StartCoroutine(ReturnParticleToPool(particle, particle.main.duration));
    }

    private ParticleSystem CreateParticle()
    {
        ParticleSystem currentParticle = Instantiate(_prefab, transform);
        _particlesPool.Enqueue(currentParticle);

        return currentParticle;
    }

    private ParticleSystem GetParticleFromPool()
    {
        ParticleSystem particle;

        if(_particlesPool.TryPeek(out _) == false)
        {
            CreateParticle(); 
        }

        particle = _particlesPool.Dequeue();

        return particle;
    }

    public void ReturnParticleToPool(ParticleSystem particle)
    {
        _particlesPool.Enqueue(particle);
    }

    public IEnumerator ReturnParticleToPool(ParticleSystem particle, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        ReturnParticleToPool(particle);
    }
    
}
