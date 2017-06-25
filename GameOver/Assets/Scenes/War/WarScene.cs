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

    private BombEmitter BombEmitterScript;
    private WarCollectable[] Collectables;
    private int CollectablesDroppedCount;

    // Use this for initialization
    new void Start()
    {
        Debug.Log("War Start");
        base.Start();

        BombEmitterScript = BombEmitter.GetComponent<BombEmitter>();

        // Find Collectables
        CollectablesDroppedCount = 0;
        Collectables = GameObject.Find("WarCollectables").GetComponentsInChildren<WarCollectable>();

        BombEmitter.GetComponent<BombEmitter>().StartBombing();
        DropCollectable();
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
            // todo - check player position
            CollectableDropRight = false;
        }

        // Don't bomb our collectable
        if (CollectableDropRight)
        {
            BombEmitterScript.AvoidRight = true;
            dropFrom = RightDropPosition.position;
        }
        else
        {
            BombEmitterScript.AvoidLeft = true;
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
                BombEmitterScript.AvoidLeft = false;
                BombEmitterScript.AvoidRight = false;
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
