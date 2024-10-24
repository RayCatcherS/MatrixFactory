using System.Threading.Tasks;
using UnityEngine;

public class IncineratorTrigger : MonoBehaviour {

    [SerializeField] private Animator _incineratorAnimator;
    private bool incineratorOpened = false;
    private bool requestToOpenIncinerator = false;
    private readonly int packageLayer = 6;
    private readonly int physicsPackageLayer = 11;

    private float closingTimeMoment;
    private float timeBeforeClose = 0.1f;

    public void OpenIncinerator() {
        if(incineratorOpened) {
            requestToOpenIncinerator = true;
        } else {
            SetOpenIncineratorAnimation(true);
        }

        incineratorOpened = true;
    }

    public void CloseIncinerator() {

        closingTimeMoment = Time.time + timeBeforeClose;
        WaitAndCloseIncinerator();

    }


    async void WaitAndCloseIncinerator() {
        while(Time.time < closingTimeMoment) {
            await Task.Yield();

            if(requestToOpenIncinerator) {
                requestToOpenIncinerator = false;
                return;
            }
        }
        incineratorOpened = false;
        SetOpenIncineratorAnimation(false);
    }



    private void SetOpenIncineratorAnimation(bool value) {

        if(value) {
            _incineratorAnimator.SetTrigger("openIncinerator");
        } else {
            _incineratorAnimator.SetTrigger("closeIncinerator");
        }
        
    }






    private void OnTriggerEnter(Collider collider) {
        if(incineratorOpened) {
            HandleCollisionDetection(collider);
        }
    }

    private void OnTriggerStay(Collider collider) {
        if(incineratorOpened) {
            HandleCollisionDetection(collider);
        }
    }

    private void OnTriggerExit(Collider collider) {

        if(incineratorOpened) {
            if(collider.gameObject.layer == physicsPackageLayer) {
                //Debug.Log("package exited");
            }
        }
            
    }

    private void HandleCollisionDetection(Collider collider) {

        if(collider.gameObject.layer == packageLayer) {
            //Debug.Log("package enter");
            collider.gameObject.GetComponent<Package>().SetPackageAsPhysicsPackage();
        }
    }
}
