using UnityEngine;
using UnityEngine.UI;

public class GenericButton : MonoBehaviour {

    [SerializeField] private Image _buttonIncon;
    [SerializeField] private Sprite _enableIcon;
    [SerializeField] private Sprite _disableIcon;
    public void MakeButtonInteractable(bool value) {

        if (value) {
            gameObject.GetComponent<Button>().interactable = true;
            _buttonIncon.sprite = _enableIcon;
        } else {
            gameObject.GetComponent<Button>().interactable = false;
            _buttonIncon.sprite = _disableIcon;
        }
        
    }   
}
