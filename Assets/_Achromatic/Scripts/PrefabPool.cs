using System.Collections.Generic;
using UnityEngine;

public class PrefabPool {

    // public API  ==============================================================

    public GameObject Spawn() {
        // spawn an instance without changing its position
        if (availableQ.Count > 0) {
            GameObject go = availableQ.Dequeue();
            go.SetActive(true);  // activate new instance
            activeQ.Enqueue(go);  // track as newest spawned
            return go;
        }

        // recycle earliest spawned instance when pool is full
        GameObject oldest = activeQ.Dequeue();

        activeQ.Enqueue(oldest);  // now considered newest
        return oldest;
    }

    public GameObject Spawn(float x, float y) {
        GameObject go = Spawn();  // reuse generic spawn logic
        if (go != null) {
            go.transform.position = new Vector3(x, y, 0f);
        }
        return go;
    }

    // constructor  ===========================================================
    public PrefabPool(int maxPrefabsCnt, string prefabPath,
                Transform parentTransform = null) {
        maxCount = Mathf.Max(0, maxPrefabsCnt);  // ensure non-negative
        availableQ = new Queue<GameObject>(maxCount);
        activeQ = new Queue<GameObject>(maxCount);

        prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null) {
            Debug.LogError("PrefabPool:\tFailed To Load Prefab: " + prefabPath);
            return;
        }

        for (int i = 0; i < maxCount; i++) {
            GameObject go = Object.Instantiate(prefab);  // preinstantiate all
            if (parentTransform != null) {
                go.transform.SetParent(parentTransform, false);  // set parent
            }
            go.SetActive(false);  // keep inactive until spawned
            availableQ.Enqueue(go);
        }
    }

    // private members  =======================================================
    private readonly GameObject prefab;
    private readonly int maxCount;
    private readonly Queue<GameObject> availableQ;
    private readonly Queue<GameObject> activeQ;
}