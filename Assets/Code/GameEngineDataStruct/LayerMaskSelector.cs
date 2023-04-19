


using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LayerMaskSelector {

    [SerializeField] private List<int> _layers;

    public LayerMask LayerMaskIgnore() {
        LayerMask mask = 0;

        for(int i = 0; i < _layers.Count; i++) {
            mask = mask | (1 << _layers[i]);
        }

        mask = ~(mask);

        return mask;
    }
}
