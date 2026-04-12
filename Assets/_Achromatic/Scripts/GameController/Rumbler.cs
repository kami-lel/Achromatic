using UnityEngine;


[RequireComponent(typeof(GameController))]
public class Rumbler: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static Rumbler I {
        get; private set;
    }


    // Public Methods  #########################################################


    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        I = this;
    }

}
