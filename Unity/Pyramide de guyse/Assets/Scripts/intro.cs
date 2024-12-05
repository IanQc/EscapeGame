using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro : MonoBehaviour
{
    public GameObject joueur;
    public GameObject uvLight;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(overview());
        StartCoroutine(test());
    }
    private IEnumerator overview()
    {
        yield return new WaitForSeconds(7);
        Debug.Log("positionSet");
        joueur.transform.position = new Vector3 (0f, 6f, 22.5f);
        joueur.transform.rotation = Quaternion.Euler(90, 0, 0);
        yield break;
    }

    private IEnumerator test()
    {
        yield return new WaitForSeconds(10);
        Debug.Log("positionSet");
        joueur.GetComponent<Animator>().Play("shineLight");
        yield return new WaitForSeconds(5);
        joueur.GetComponent<Animator>().Play("shineOff");
        yield return new WaitForSeconds(5);
        uvLight.GetComponent<Animator>().Play("uvPowerOn");
        yield return new WaitForSeconds(5);
        uvLight.GetComponent<Animator>().Play("uvPowerOff");
        yield break;
    }
}
