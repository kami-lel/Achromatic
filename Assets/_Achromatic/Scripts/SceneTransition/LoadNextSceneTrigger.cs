using Assets._Achromatic.Scripts.Players;
using UnityEngine;

public class LoadNextSceneTrigger: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField]
    private string nextSceneName;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        player = GCS.FindPlayer().GetComponent<Player>();

        if (nextSceneName == null || nextSceneName == "") {
            Debug.LogError("must set: Next Scene Name", this);
        }
    }

    private void Start() {
        player.OnTriggerEnter += HandleOnTriggerEnter;
    }

    // event handler  ######################################################

    private void HandleOnTriggerEnter(string triggerTag) {

        if ((GCS.I.states & GameState.EXPLORE_CONTROL) != 0 &&
                triggerTag == TRIGGER_TAG) {

            GCS.I.LoadNextScene(nextSceneName);
        }
    }
    // constants  ##############################################################
    private const string TRIGGER_TAG = "LoadNextSceneTrigger";

    // private members  ########################################################
    // Cached References
    private Player player;
}
