using UnityEngine;

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
            if (camTransform == null)
                Debug.LogError("FloatsManager: failed to find Main Camera Transform");

            rockPool = new(16, PREFAB_FOLDER + "FloatingRock", transform);
            notePool = new(8, PREFAB_FOLDER + "FloatingNote", transform);

            // Pre-warm: fill the entire visible window on startup
            float leftEdge = camTransform.position.x - renderDistanceX;
            float rightEdge = camTransform.position.x + renderDistanceX;

            nextRockSpawnX = leftEdge;
            nextNoteSpawnX = leftEdge;

            SpawnRocksAhead(rightEdge);
            SpawnNotesAhead(rightEdge);
        }

        private void Update() {
            float rightEdge = camTransform.position.x + renderDistanceX;

            // If camera teleported far ahead, skip spawn cursor forward
            // to avoid a massive catch-up loop in one frame
            ClampSpawnCursorToWindow(rightEdge);

            SpawnRocksAhead(rightEdge);
            SpawnNotesAhead(rightEdge);

        }

        // Private Methods  ####################################################

        /// <summary>
        /// If the camera has jumped so far right that the spawn cursors are
        /// more than one full window behind, snap them forward.
        /// This prevents a multi-thousand iteration catch-up loop.
        /// </summary>
        private void ClampSpawnCursorToWindow(float rightEdge) {
            float maxLag = renderDistanceX * 2f;    // one full window width

            if (rightEdge - nextRockSpawnX > maxLag)
                nextRockSpawnX = rightEdge - maxLag;

            if (rightEdge - nextNoteSpawnX > maxLag)
                nextNoteSpawnX = rightEdge - maxLag;
        }

        private void SpawnRocksAhead(float rightEdge) {
            while (nextRockSpawnX <= rightEdge) {
                float y = Random.Range(spawnMinY, spawnMaxY);
                rockPool.Spawn(nextRockSpawnX, y, key: 0);
                nextRockSpawnX += Random.Range(rockSpawnIntervalMinX, rockSpawnIntervalMaxX);
            }
        }

        private void SpawnNotesAhead(float rightEdge) {
            while (nextNoteSpawnX <= rightEdge) {
                float y = Random.Range(spawnMinY, spawnMaxY);
                notePool.Spawn(nextNoteSpawnX, y, key: 0);
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