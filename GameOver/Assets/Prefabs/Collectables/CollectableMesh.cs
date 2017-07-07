using UnityEngine;

public class CollectableMesh : MonoBehaviour
{
    public float SpinSpeed = 250;

    private WarCollectable WarCollectable;
    private Quaternion InitaialRotation;
    private MeshRenderer Mesh;

    private void Awake()
    {
        Mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        WarCollectable = this.gameObject.GetComponent<WarCollectable>();
        WarCollectable.OnCollected += CoinCollected;
        WarCollectable.OnDestroyed += CoinDestroyed;
        WarCollectable.OnHide += WarCollectable_OnHide;
        WarCollectable.OnShow += WarCollectable_OnShow;
    }

    private void WarCollectable_OnShow(object sender, System.EventArgs e)
    {
        Mesh.enabled = true;
    }

    private void WarCollectable_OnHide(object sender, System.EventArgs e)
    {
        Mesh.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        InitaialRotation = this.transform.localRotation;
        if (WarCollectable.Collected)
        {
            CoinCollected(null, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SpinSpeed != 0)
        {
            transform.Rotate(0, SpinSpeed * Time.deltaTime, 0);
        }
    }

    void CoinCollected(object sender, object args)
    {
        SpinSpeed = 0;
        transform.localRotation = InitaialRotation;
    }

    void CoinDestroyed(object sender, object args)
    {

    }
}
