

using System;
using System.Collections.Generic;
using UnityEngine;


public class BeatmapPrefabsPool: IDisposable {

    // constants  ==============================================================
    private const string ROOT_OBJECT_NAME = "BeatmapPrefabsPool";

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

    // private properties  =====================================================
    private static Dictionary<string, GameObject> prefabs;

    /// <summary>
    /// i.e. "BeatmapPrefabsPool" appears in Hierarchy during runtime
    /// </summary>
    private readonly GameObject root;

    /// <summary>
    /// pools of all Prefab objects across pieces
    /// </summary>
    private static Dictionary<GameObject, Queue<GameObject>> pools;

    // constructor & destructor  ===============================================

    /// <summary>
    /// instantiate during <c>Awake()</c>
    /// </summary>
    /// <param name="parentTransform">
    /// transform which the BeatmapElementsPool will be placed under
    /// </param>
    public BeatmapPrefabsPool(Transform parentTransform) {
        // create root object  -------------------------------------------------
        root = new GameObject(ROOT_OBJECT_NAME);
        root.transform.SetParent(parentTransform, false);

        // load prefabs from resources  ----------------------------------------
        if (prefabs == null) {
            prefabs = new Dictionary<string, GameObject>();

            // load Prefabs by types
            for (int i = 0; i < ELEMENTS_NAMES.Length; i++) {
                string key = ELEMENTS_NAMES[i];
                string path = PREFAB_FOLDER_PATH + key;
                GameObject prefab = Resources.Load<GameObject>(path);

                if (prefab == null) {
                    Debug.LogError(
            "BeatmapPrefabPools: missing Prefab: Resources/" + path);
                    continue;
                }

                prefabs[key] = prefab;
            }
        }

        // create all instances  -----------------------------------------------
        if (pools == null) {
            pools = new Dictionary<GameObject, Queue<GameObject>>();

            // per element type
            foreach (GameObject prefab in prefabs.Values) {
                Queue<GameObject> q = new();

                for (int i = 0; i < 10; i++) {
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

    ~BeatmapPrefabsPool() {
        Clear();
    }

    // IDisposable Implementation  =============================================
    public void Dispose() {
        Clear();
        GC.SuppressFinalize(this);
    }

    // public methods  =========================================================
    public GameObject Spawn(GameObject prefab, Vector2 pos) {
        if (!pools.TryGetValue(prefab, out var q)) {
            Debug.LogError("BeatmapPrefabPools: must be Beatmap Prefab, not: "
                    + prefab);
            return null;
        }

        GameObject go = q.Dequeue();
        go.SetActive(true);
        go.transform.position = pos;

        return go;
    }

    private void Clear() {
        // todo
    }


}