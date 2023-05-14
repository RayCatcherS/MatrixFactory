using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestoyEffect : MonoBehaviour {

    [SerializeField] private Transform particleOrign;
    [SerializeField] private string particleEffectPoolId;


    public void StartDestroyEffect() {
        string particleObjectDestroyedPoolId = particleEffectPoolId;
        GameObject particle = PrefabManager.Instance.SpawnFromPool(
            particleObjectDestroyedPoolId,
            particleOrign.transform.position,
            Quaternion.identity
        );
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        if(particleSystem != null) {
            particleSystem.Play();
        }
    }
}
