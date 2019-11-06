using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCtrl : MonoBehaviour
{
    public static LaserCtrl instance_LayerCtrl;

    // Start is called before the first frame update
    private LineRenderer render;
    private GameObject pointerPrefab;
    private GameObject pointer;
    private HinderDestroy hiderObject;


    public static Ray ray;
    public static RaycastHit hit;
    public float dist = 100.0f;

    public GameObject noSelectChoice;

    //private Transform tr;

    void Awake()
    {
        instance_LayerCtrl = this;
    }

    void Start()
    {

    }

    public void RayShoot()
    {
        Debug.DrawLine(transform.position, hit.point, Color.red, dist);

        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, dist, 1 << LayerMask.NameToLayer("HINDER")))
        {
            Debug.Log("사물이 잡혔습니다.");
            HinderDestroy.instance_hiderDestroy.FindHidden();
            //사물이 잡히면 GameManager에서 FinderWinGame() 코루틴 실행
            StartCoroutine(GameManager.instance_GM.FinderWinGame());
            //MoveCtrl.isNotMove = true;

        }
        if (Physics.Raycast(ray, out hit, dist, 1 << LayerMask.NameToLayer("NOHINDER")))
        {
            Vector3 pos = hit.transform.position;
            pos.z = pos.z - 1;
            pos.y = pos.y + hit.transform.localScale.y;
            Instantiate(noSelectChoice, pos, hit.transform.rotation);
            
        }
    }
}
