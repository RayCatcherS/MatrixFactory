using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PackageDamageEffect : MonoBehaviour {

    [SerializeField] private Transform damageOrigin;
    [SerializeField] float explosionForce = 10;
    [SerializeField] float explosionRadius = 5;

    [SerializeField] private AnimationClip alpha1;
    [SerializeField] private AnimationClip alpha1To0;

    void OnDisable() {

        Init();

    }

    void OnEnable() {
        StartEffectAsync();
        
    }

    void Init() {
        Rigidbody[] rbs = gameObject.GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < rbs.Length; i++) {

            rbs[i].gameObject.transform.localPosition = Vector3.zero;
            rbs[i].gameObject.transform.localRotation = Quaternion.identity;
        }
        gameObject.GetComponent<Animation>().clip = alpha1;
        gameObject.GetComponent<Animation>().Play();
    }

    private async Task StartEffectAsync() {
        Rigidbody[] rbs = gameObject.GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < rbs.Count(); i++) {

            if(rbs[i]) {


                Vector3 forceOriginRandomOffset = new Vector3(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(-0.5f, 0.5f)
                );


                rbs[i].AddExplosionForce(explosionForce, damageOrigin.position + forceOriginRandomOffset, explosionRadius, 0.5f);
            }
        }

        gameObject.GetComponent<Animation>().clip = alpha1To0;
        gameObject.GetComponent<Animation>().Play();

        while(gameObject.GetComponent<Animation>().isPlaying) {
            await Task.Yield();
        }
        gameObject.SetActive(false);
    }
}
