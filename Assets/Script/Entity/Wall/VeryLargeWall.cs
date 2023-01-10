using UnityEngine;


public class VeryLargeWall : MonoBehaviour
{
    public GameObject hoop;

    private void OnEnable()
    {
        if (Time.frameCount < 3) return;
        hoop = ObjectPooler.Instance.Spawn("Hoop");
        hoop.transform.SetParent(transform);
        hoop.GetComponent<Hoop>().Appearance(Vector3.right * 2.5f + Vector3.up * 2f);
        //hoop.transform.position = transform.position + Vector3.up * -1.5f
        //                               + Vector3.right * 3.5f * HoopSpawner.Instance.currentZone * -1f;
        hoop.transform.localPosition = Vector3.right * Random.Range(7.5f, 8.5f) *
                                  HoopSpawner.Instance.currentZone;
    }

    private void Start()
    {
        this.RegisterListener(EventID.HoopPassed, (param) => OnHoopPassed());
    }

    private void OnHoopPassed()
    {
        if (hoop.GetComponent<Hoop>().containsBall)
        {
            hoop.transform.SetParent(null);
            GetComponent<Disappearance>().Excute();
        }
    }
}
