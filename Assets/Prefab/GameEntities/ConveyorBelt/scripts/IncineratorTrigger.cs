using System.Threading.Tasks;
using UnityEngine;

public class IncineratorTrigger : MonoBehaviour {

    [SerializeField] private Animator _incineratorAnimator;
    private bool incineratorOpened = false;
    private bool requestToOpenIncinerator = false;
    private readonly int packageLayer = 6;

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
        Debug.Log(Time.time + " : " + closingTimeMoment);
        while(Time.time < closingTimeMoment) {
            Debug.Log("waiting to close incinerator");
            await Task.Yield();

            if(requestToOpenIncinerator) {
                requestToOpenIncinerator = false;
                return;
            }
        }

        Debug.Log("closing incinerator");

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

    private void HandleCollisionDetection(Collider collider) {

        if(collider.gameObject.layer == packageLayer) {
            Debug.Log("incinerator package trigger detected");
            collider.gameObject.GetComponent<Package>().SetPackageAsPhysicsPackage();
        }
    }
}
