using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

static public class DataDeepCopy {
    static public Vector2Int DeepCopy(Vector2Int obj) {
        return new Vector2Int(obj.x, obj.y);
    }
}
