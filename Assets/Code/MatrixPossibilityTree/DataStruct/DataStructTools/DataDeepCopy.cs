using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

static public class DataDeepCopy {
    static public Vector2Int DeepCopy(Vector2Int obj) {
        return new Vector2Int(obj.x, obj.y);
    }
	static public List<Vector2Int> DeepCopy(List<Vector2Int> obj) {
		List<Vector2Int> copiedVal = new List<Vector2Int>();

		for(int i = 0; i < obj.Count; i++) {
			copiedVal.Add(new Vector2Int(obj[i].x, obj[i].y));
		}

		return copiedVal;
	}
}
