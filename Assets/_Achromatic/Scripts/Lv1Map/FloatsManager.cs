using UnityEngine;

namespace Assets._Achromatic.Scripts.Lv1Map {

    public class FloatsManager: MonoBehaviour {


        // Inspector Fields  ###################################################

        [SerializeField]
        private float renderDistanceX;

        // MonoBehavior Lifecycle  #############################################
        private void Awake() {
            camTransform = Camera.main.transform;
            if (camTransform == null) {
                Debug.LogError("fail to find Main Camera Transform");
            }

            // init pools
            rockPool = new(16, PREFAB_FOLDER + "FloatingRock", transform);
            notePool = new(8, PREFAB_FOLDER + "FloatingNote", transform);
        }

        private void Update() {
            // TODO spawn prefabs
        }

        // constants  ##########################################################
        private const string PREFAB_FOLDER = "Prefabs/Lv1Map/";

        // private members  ####################################################

        private PrefabPool<int> rockPool;
        private PrefabPool<int> notePool;

        // cached references
        private Transform camTransform;
    }

}

