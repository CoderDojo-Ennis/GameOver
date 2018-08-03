using System.Linq;
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
    public Vector3 PlayerStartPosition;

    [Header("Bombs")]
    public GameObject BombEmitter;

    private BombEmitter BombEmitterScript;
    private WarCollectable[] Collectables;
    private WarCollectable LastCollectable;
    private int CollectablesDroppedCount;

    // Use this for initialization
    new void Start()
    {
        //Debug.Log("War Start");
        base.Start();
    }

    public override void FirstUpdate()
    {
        base.FirstUpdate();
        PlayerScript.Instance.transform.rotation = Quaternion.identity;
        PlayerScript.Instance.transform.position = PlayerStartPosition;

        BombEmitterScript = BombEmitter.GetComponent<BombEmitter>();
        PlayerScript.Instance.ShowKinect(1);

        GetComponentInChildren<ChildScript>().StartFollowing(PlayerScript.Instance.transform);

        // Find Collectables not collected
        CollectablesDroppedCount = 0;
        var allCollectables = GameObject.Find("WarCollectables").GetComponentsInChildren<WarCollectable>();
        Collectables = allCollectables.Where(c => c.Collected == false).ToArray();
        Debug.Log("Items to collect: " + Collectables.Length);
        if (Collectables.Length < 1)
        {
            // Start over if nothing left
            Collectables = allCollectables;
            Debug.Log("Revised Items to collect: " + Collectables.Length);
            foreach (var collectable in Collectables)
            {
                collectable.Collected = false;
                collectable.Hide();
            }
        }
        LastCollectable = Collectables[Collectables.Length - 1];

        // todo: delete this
        //LastCollectable = Collectables[0];

        // Watch for last collectable collected or destroyed
        LastCollectable.OnCollected += CollectablesDone;
        LastCollectable.OnDestroyed += CollectablesDone;

        // Delay so this happens after invitation so we can get the player start X
        this.Delay(0.2f, () =>
        {
            // Start dropping stuff
            BombEmitter.GetComponent<BombEmitter>().StartBombing();
            DropCollectable();
        });

        // When the player dies
        PlayerScript.Instance.OnDeath += WarScene_OnDeath;
    }

    private void WarScene_OnDeath(object sender, System.EventArgs e)
    {
        RemoveBombs();
    }

    // un-hook any events
    internal new void OnDestroy()
    {
        base.OnDestroy();
        PlayerScript.Instance.OnDeath -= WarScene_OnDeath;
    }

    /// <summary>
    /// Delete all the bombs, and stop the emitter
    /// </summary>
    private void RemoveBombs()
    {
        //Debug.Log("Remove Bombs");

        // No more bombs
        BombEmitterScript.enabled = false;
        foreach (var bomb in GameObject.FindGameObjectsWithTag("Bomb"))
        {
            Destroy(bomb);
        }
    }

    /// <summary>
    /// The last collectable was either collected or destroyed
    /// </summary>
    /// <param name="sender">The collectable</param>
    /// <param name="e">Nothin</param>
    private void CollectablesDone(object sender, System.EventArgs e)
    {
        RemoveBombs();

        AvatarScript avatar = PlayerScript.Instance.ChangeToAvatar();
        avatar.SetAnimation("Idle");
        this.Delay(1.5f, () =>
        {
            avatar.SetAnimation("WalkRight");
            float screenRitghtX = 7;
            float avatarX = avatar.transform.position.x;
            float distanceToRight = screenRitghtX - avatarX;
            avatar.GlideX(avatarX, screenRitghtX, distanceToRight * .2f).Then(FadeToBeach);
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
                //FadeToScene("SeaScene");

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
            // Drop first bomb right on player
            BombEmitterScript.DropNextBombFrom(new Vector3(PlayerScript.Instance.transform.localPosition.x, 4.27f, 0));
            // Make it a challenge for the player
            float playerX = PlayerScript.Instance.transform.localPosition.x;
            Debug.Log("playerX=" + playerX);
            CollectableDropRight = (playerX < 0);
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
