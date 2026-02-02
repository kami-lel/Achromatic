
using System;
using System.Collections.Generic;
using UnityEngine;


public class BeatmapElementsPool: IDisposable {

    // constants  **********************************************************
    /// <summary>
    /// name of <c>GameObject</c> shown in Hierarchy
    /// </summary>
    private const string GAME_OBJECT_NAME = "BeatmapElementsPoolRoot";

    /// <summary>
    /// folder which contains all elements Prefabs in Resources
    /// </summary>
    private const string PREFAB_FOLDER_PATH =
            "Prefabs/BeatmapElements/";

    /// <summary>
    /// element prefab names under "Prefabs/BeatmapElements/"
    /// </summary>
    private static readonly string[] ELEMENTS_NAMES =
            { "Barline", "BeatLine" };

    /// <summary>
    /// singleton collection of prefabs
    /// </summary>
    private static Dictionary<string, GameObject> prefabs;

    /// <summary>
    /// pools of all Prefab objects across pieces
    /// </summary>
    private static Dictionary<GameObject, Queue<GameObject>> pools;


    // private members  ****************************************************
    private readonly GameObject root;
    private readonly HashSet<GameObject> activeInstances;

    // constructor & destructor  *******************************************

    /// <summary>
    /// instantiate during <c>Awake()</c>
    /// </summary>
    /// <param name="parentTransform">
    /// transform which the BeatmapElementsPool will be placed under
    /// </param>
    public BeatmapElementsPool(Transform parentTransform) {
        activeInstances = new HashSet<GameObject>();

        // create pool root  -----------------------------------------------
        root = new GameObject(GAME_OBJECT_NAME);
        root.transform.SetParent(parentTransform, false);

        // load prefabs from Resources if non existent
        if (prefabs == null) {
            prefabs = new Dictionary<string, GameObject>();

            // load Prefabs by types
            for (int i = 0; i < ELEMENTS_NAMES.Length; i++) {
                string key = ELEMENTS_NAMES[i];
                string path = PREFAB_FOLDER_PATH + key;
                GameObject prefab = Resources.Load<GameObject>(path);

                if (prefab == null) {
                    Debug.LogError(
            "PieceScript: missing Prefab: Resources/" + path);
                    continue;
                }

                prefabs[key] = prefab;
            }
        }

        // prewarm  --------------------------------------------------------
        if (pools == null) {
            pools = new Dictionary<GameObject, Queue<GameObject>>();

            // per element type
            foreach (GameObject prefab in prefabs.Values) {
                Queue<GameObject> q = new Queue<GameObject>();

                for (int i = 0; i < 15; i++) {
                    // todo instead of set amount of 15 instances
                    GameObject go = GameObject.Instantiate(prefab);
                    go.SetActive(false);
                    go.transform.SetParent(root.transform, false);
                    q.Enqueue(go);
                }

                // add to pools
                pools[prefab] = q;
            }
        }
    }

    ~BeatmapElementsPool() {
        Clear();
    }

    // public methods  *****************************************************

    public GameObject Spawn(GameObject prefab, Vector2 pos) {
        if (!pools.TryGetValue(prefab, out Queue<GameObject> q)) {
            Debug.LogError("BeatmapElementsPool: prefab not in pool: "
                    + prefab);
            return null;
        }

        GameObject go;
        go = q.Dequeue();
        // make active
        go.transform.position = pos;
        go.SetActive(true);

        activeInstances.Add(go);
        return go;
    }

    private bool DeSpawn(GameObject go) {
        if (!activeInstances.Remove(go)) {
            Debug.LogError("BeatmapElementsPool: instance not in pool: "
                    + go);
            return false;
        }

        go.SetActive(false);
        string prefabName = go.name.Replace("(Clone)", "").Trim();

        GameObject prefab = prefabs[prefabName];
        pools[prefab].Enqueue(go);

        return true;
    }

    public void Clear() {
        // todo
    }

    // IDisposable Implementation  *****************************************
    public void Dispose() {
        Clear();
        GC.SuppressFinalize(this);
    }



    /* hack

    // Recycle Instance  =================================================
    // deactivate and return to its prefab queue, parent to pool root
    public void Recycle(GameObject instance) {
        if (instance == null)
            return;
        if (!activeInstances.Remove(instance)) {
            // Not tracked as active, still safe to recycle
        }

        // try find matching prefab key by comparing prefab name prefix
        // NOTE: store a mapping if prefab->instance link required
        instance.SetActive(false);
        instance.transform.SetParent(root.transform, false);

        // fallback: place into any queue for same prefab reference
        // attempt to find the queue whose prefab name matches
        foreach (var kv in pools) {
            if (kv.Key.name == instance.name.Replace("(Clone)", "").Trim()) {
                kv.Value.Enqueue(instance);
                return;
            }
        }

        // if no matching pool, create a general bucket for this instance
        if (!pools.TryGetValue(instance, out var newQ)) {
            newQ = new Queue<GameObject>();
            pools[instance] = newQ;
        }
        newQ.Enqueue(instance);
    }

    // Clear and Destroy All  ============================================
    public void Clear() {
        // destroy active instances first  --------------------------------
        foreach (var inst in activeInstances) {
            if (inst != null)
                GameObject.Destroy(inst);
        }
        activeInstances.Clear();

        // destroy pooled objects and clear queues  -----------------------
        foreach (var kv in pools) {
            var q = kv.Value;
            while (q.Count > 0) {
                var go = q.Dequeue();
                if (go != null)
                    GameObject.Destroy(go);
            }
        }
        pools.Clear();

        // destroy root GameObject  -------------------------------------
        if (root != null)
            GameObject.Destroy(root);
    }


*/


}