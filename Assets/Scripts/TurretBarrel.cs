using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBarrel : MonoBehaviour
{
    [field:SerializeField] public Transform shootPoint { get; private set; }
    public float strenght = 0.4f;
    public float time = 0.16f;
    public void Recoil()
    {
        LeanTween.cancel(gameObject);
        transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y , 0);
        LeanTween.moveLocalZ(gameObject, -strenght, time).setEaseOutCubic().setLoopPingPong(1);
    }
}
