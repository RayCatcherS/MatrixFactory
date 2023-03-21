using Unity.VisualScripting;
using UnityEngine;

namespace PT.DataStruct {
    public class PossibilityItem : MonoBehaviour
    {
        public PossibilityItem(string id)
        {
            _id = id;
        }
        private string _id;
        public string id
        {
            get { return _id; }
        }

        public override string ToString()
        {
            return _id;
        }
    }
}
