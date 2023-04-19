using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PT.DataStruct {
    public interface Path {
        public string id();

        public Vector2Int MatrixSize();
        public Vector2Int LastPathPos();

        public Vector2Int EndPathPosition();
        public Vector2Int StartPathPosition();
    }
}
    
