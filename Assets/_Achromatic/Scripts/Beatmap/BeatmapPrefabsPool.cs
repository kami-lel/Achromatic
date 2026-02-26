

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
            { "Barline", "BeatLine", "JumpNote", "DashNote"};

    // private properties  =====================================================
    private static Dictionary<string, GameObject> prefabs;

    /// <summary>
    /// i.e. "BeatmapPrefabsPool" appears in Hierarchy during runtime
    /// </summary>
    private readonly GameObject root;

    /// <summary>
    /// pools of all Prefab objects across pieces
    /// </summary>
    private static Dictionary<string, Queue<GameObject>> pools;

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
            pools = new();

            // per element type
            foreach (var key_value in prefabs) {
                String prefabName = key_value.Key;
                GameObject prefab = key_value.Value;

                Queue<GameObject> q = new();

                // Todo not set amount
                int cnt = prefabName switch {
                    "BeatLine" => 30,
                    _ => 10
                };

                for (int i = 0; i < cnt; i++) {
                    GameObject go = GameObject.Instantiate(prefab);
                    go.SetActive(false);
                    go.transform.SetParent(root.transform, false);
                    q.Enqueue(go);
                }

                // add to pools
                pools[prefabName] = q;
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
    public GameObject Spawn(string prefab, Vector2 pos) {
        if (!pools.TryGetValue(prefab, out var q)) {
            Debug.LogWarning("BeatmapPrefabPools: fail to spawn, "
                    + "must be Beatmap Prefabs, not: "
                    + prefab);
            return null;
        }

        GameObject go = q.Dequeue();
        go.SetActive(true);
        go.transform.position = pos;

        q.Enqueue(go);  // add back to the queue
        return go;
    }

    private void Clear() {
        // Todo
    }


}