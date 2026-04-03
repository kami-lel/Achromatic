using UnityEngine;

// BUG BUG some rocks are randomly flying

namespace Assets._Achromatic.Scripts.Lv1Map {

    public class FloatsManager: MonoBehaviour {

        // Inspector Fields  ###################################################
        [SerializeField] private float renderDistanceX = 20f;
        [SerializeField] private float rockSpawnIntervalMinX = 1.5f;
        [SerializeField] private float rockSpawnIntervalMaxX = 5f;
        [SerializeField] private float noteSpawnIntervalMinX = 4f;
        [SerializeField] private float noteSpawnIntervalMaxX = 10f;
        [SerializeField] private float spawnMinY = -4f;
        [SerializeField] private float spawnMaxY = 4f;

        // MonoBehaviour Lifecycle  ############################################
        private void Awake() {
            camTransform = Camera.main.transform;
            if (camTransform == null) {
                Debug.LogError("FloatsManager: failed to find Main Camera Transform");
                return;
            }

            rockPool = new PrefabPool<int>(16, PREFAB_FOLDER + "FloatingRock", transform);
            notePool = new PrefabPool<int>(8, PREFAB_FOLDER + "FloatingNote", transform);

            // Pre-warm: seed the entire visible window on startup
            float leftEdge = camTransform.position.x - renderDistanceX;
            float rightEdge = camTransform.position.x + renderDistanceX;

            nextRockSpawnX = leftEdge;
            nextNoteSpawnX = leftEdge;

            SpawnRocksUpTo(rightEdge);
            SpawnNotesUpTo(rightEdge);
        }

        private void OnDestroy() {
            rockPool?.Dispose();
            notePool?.Dispose();
        }

        private void Update() {
            float rightEdge = camTransform.position.x + renderDistanceX;

            // If camera teleported far ahead, skip spawn cursors forward
            // to avoid a massive catch-up loop in one frame
            ClampSpawnCursors(rightEdge);

            SpawnRocksUpTo(rightEdge);
            SpawnNotesUpTo(rightEdge);
        }

        // Private Methods  ####################################################

        /// <summary>
        /// Snap cursors forward if the camera jumped more than one full window
        /// ahead, preventing a multi-thousand-iteration catch-up loop.
        /// </summary>
        private void ClampSpawnCursors(float rightEdge) {
            float maxLag = renderDistanceX * 2f;
            if (rightEdge - nextRockSpawnX > maxLag)
                nextRockSpawnX = rightEdge - maxLag;
            if (rightEdge - nextNoteSpawnX > maxLag)
                nextNoteSpawnX = rightEdge - maxLag;
        }

        private void SpawnRocksUpTo(float rightEdge) {
            while (nextRockSpawnX <= rightEdge) {
                rockPool.Spawn(nextRockSpawnX, Random.Range(spawnMinY, spawnMaxY), key: 0);
                nextRockSpawnX += Random.Range(rockSpawnIntervalMinX, rockSpawnIntervalMaxX);
            }
        }

        private void SpawnNotesUpTo(float rightEdge) {
            while (nextNoteSpawnX <= rightEdge) {
                notePool.Spawn(nextNoteSpawnX, Random.Range(spawnMinY, spawnMaxY), key: 0);
                nextNoteSpawnX += Random.Range(noteSpawnIntervalMinX, noteSpawnIntervalMaxX);
            }
        }

        // Constants  ##########################################################
        private const string PREFAB_FOLDER = "Prefabs/Lv1Map/";

        // Private Members  ####################################################
        private PrefabPool<int> rockPool;
        private PrefabPool<int> notePool;
        private Transform camTransform;
        private float nextRockSpawnX;
        private float nextNoteSpawnX;
    }
}