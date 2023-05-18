using UnityEngine;

public class ObjectDestoyEffect : MonoBehaviour {

    [SerializeField] private Transform particleOrign;
    [SerializeField] private string _particleEffectPoolId;
    [SerializeField] private string _substituteOfDestroyedObjectPoolId;


    public string particleEffectPoolId {
        set { _particleEffectPoolId = value; }
    }
    public string substituteOfDestroyedObjectPoolId {
        set { _substituteOfDestroyedObjectPoolId = value; }
    }

    public void StartDestroyEffect() {

        if(_particleEffectPoolId != "") {

            GameObject particle = PrefabManager.Instance.SpawnFromPool(
                        _particleEffectPoolId,
                        particleOrign.transform.position,
                        Quaternion.identity
                    );
            ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
            if(particleSystem != null) {
                particleSystem.Play();
            }
        }
        

        if(_substituteOfDestroyedObjectPoolId != "") {
            SetSobstitute();
        }
    }

    private void SetSobstitute() {
        string substituteOfDestroyedObjectPoolId = _substituteOfDestroyedObjectPoolId;
        GameObject substitute = PrefabManager.Instance.SpawnFromPool(
            substituteOfDestroyedObjectPoolId,
            transform.position,
            Quaternion.identity
        );
    }
}
