using System;
using System.Collections.Generic;
using UnityEngine;


public class PrefabPool: IDisposable {

    // public API  #############################################################

    public GameObject Spawn() {
        if (isDisposed) {
            Debug.LogError("PrefabPool:\tAttempt To Spawn After Dispose");
            return null;
        }

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

    // con/destructor  #########################################################
    public PrefabPool(int maxPrefabsCnt, string prefabPath,
                      Transform parentTransform = null) {
        maxCount = Math.Max(0, maxPrefabsCnt);  // ensure non-negative
        availableQ = new Queue<GameObject>(maxCount);
        activeQ = new Queue<GameObject>(maxCount);

        prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null) {
            Debug.LogError("PrefabPool: Failed To Load Prefab: " + prefabPath);
            return;
        }

        for (int i = 0; i < maxCount; i++) {
            // preinstantiate all
            GameObject go = UnityEngine.Object.Instantiate(prefab);
            if (parentTransform != null) {
                go.transform.SetParent(parentTransform, false);  // set parent
            }
            go.SetActive(false);  // keep inactive until spawned
            availableQ.Enqueue(go);
        }
    }

    ~PrefabPool() {
        Dispose(false);  // finalizer does not attempt Unity destroys
    }

    // implement IDisposable  ##################################################
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (isDisposed)
            return;
        isDisposed = true;

        if (disposing) {
            // Safe to destroy Unity objects on main thread destroy available instances
            if (availableQ != null) {
                foreach (GameObject go in availableQ) {
                    if (go == null)
                        continue;
                    if (Application.isPlaying) {
                        UnityEngine.Object.Destroy(go);
                    } else {
                        UnityEngine.Object.DestroyImmediate(go);
                    }
                }
                availableQ.Clear();
            }

            // Destroy active instances  ---------------------------------------
            if (activeQ != null) {
                foreach (GameObject go in activeQ) {
                    if (go == null)
                        continue;
                    if (Application.isPlaying) {
                        UnityEngine.Object.Destroy(go);
                    } else {
                        UnityEngine.Object.DestroyImmediate(go);
                    }
                }
                activeQ.Clear();
            }

        } else {
            // finalizer path: do not call Unity API from background thread
            Debug.LogWarning("PrefabPool:\t" +
                    "Dispose was not called before GC. " +
                    "Unity objects were not destroyed safely.");
        }
    }

    // private members  ########################################################
    private readonly GameObject prefab;
    private readonly int maxCount;
    private readonly Queue<GameObject> availableQ;
    private readonly Queue<GameObject> activeQ;
    private bool isDisposed = false;
}