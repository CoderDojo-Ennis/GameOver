using UnityEngine;

public class WarScene : BaseGameScene
{
    [Header("Collectables")]
    public float CollectableDelaySeconds = 10;
    public float CollectableDelayRaondomSeconds = 3;
    public float CollectableLifeSeconds = 3;
    public bool CollectableDropRight = false;
    public Transform LeftDropPosition;
    public Transform RightDropPosition;

    [Header("Bombs")]
    public GameObject BombEmitter;

    [Header("Player")]
    public Transform SpineBase;

    private BombEmitter BombEmitterScript;
    private WarCollectable[] Collectables;
    private WarCollectable LastCollectable;
    private int CollectablesDroppedCount;

    // Use this for initialization
    new void Start()
    {
        Debug.Log("War Start");
        base.Start();

        BombEmitterScript = BombEmitter.GetComponent<BombEmitter>();
        PlayerScript.Instance.ShowKinect(1);

        // Find Collectables
        CollectablesDroppedCount = 0;
        Collectables = GameObject.Find("WarCollectables").GetComponentsInChildren<WarCollectable>();
        LastCollectable = Collectables[Collectables.Length - 1];

        // todo: delete this
        LastCollectable = Collectables[0];

        // Watch for last collectable collected or destroyed
        LastCollectable.OnCollected += CollectablesDone;
        LastCollectable.OnDestroyed += CollectablesDone;

        // Start dropping stuff
        BombEmitter.GetComponent<BombEmitter>().StartBombing();
        DropCollectable();
    }

    /// <summary>
    /// The last collectable was either collected or destroyed
    /// </summary>
    /// <param name="sender">The collectable</param>
    /// <param name="e">Nothin</param>
    private void CollectablesDone(object sender, System.EventArgs e)
    {
        // No more bombs
        BombEmitterScript.enabled = false;
        foreach (var bomb in GameObject.FindGameObjectsWithTag("Bomb"))
        {
            Destroy(bomb);
        }

        var avatar = PlayerScript.Instance.ChangeToAvatar();

        this.Delay(4, () =>
        {
            FadeToBeach();
        });
    }

    // Fade to beach
    private void FadeToBeach()
    {
        this.Delay(2, () =>
        {
            GameManager.Instance.FadeToScene("BeachScene", this.FadeSeconds);
        });
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (!GameManager.Instance.Paused)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                FadeToScene("SeaScene");

                //var seaScene = GameManager.Instance.Sea;
                //GameManager.Instance.ShowScene(seaScene);
            }
        }
    }

    /// <summary>
    /// Schedule the drop
    /// </summary>
    public void DropCollectable()
    {
        Vector3 dropFrom;

        // Where to do the first one
        if (CollectablesDroppedCount == 0)
        {
            // Make it a challenge for the player
            CollectableDropRight = (SpineBase.localPosition.x < 0);
        }

        // Don't bomb our collectable
        if (CollectableDropRight)
        {
            BombEmitterScript.AvoidRight = true;
            BombEmitterScript.AvoidLeft = false;
            dropFrom = RightDropPosition.position;
        }
        else
        {
            BombEmitterScript.AvoidLeft = true;
            BombEmitterScript.AvoidRight = false;
            dropFrom = LeftDropPosition.position;
        }

        this.Delay(CollectableDelaySeconds + Random.Range(0, CollectableDelayRaondomSeconds), () =>
        {
            // Which object to enable and drop
            var newCollectable = this.Collectables[CollectablesDroppedCount];
            newCollectable.DropFrom(dropFrom);

            // Allow bombs
            this.Delay(CollectableLifeSeconds, () =>
            {
                BombEmitterScript.DropNextBombFrom(dropFrom);
            });

            // Done yet?
            CollectablesDroppedCount++;

            // Schedule the next one
            if (CollectablesDroppedCount < Collectables.Length)
            {
                // Alternate side
                CollectableDropRight = !CollectableDropRight;
                DropCollectable();
            }
        });
    }
}
