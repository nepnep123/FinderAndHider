using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HinderDestroy : MonoBehaviour
{
    private int hashIsFind = Animator.StringToHash("IsFind");
    
    public static HinderDestroy instance_hiderDestroy;

    //필요한 컴포넌트 
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        instance_hiderDestroy = this;
        anim = GetComponent<Animator>();
    }


    public void FindHidden()
    {
        //Debug.Log("Object : " + msg);
        anim.SetBool(hashIsFind, true);
        StartCoroutine(GameManager.instance_GM.FinderWinGame());
    }
}
