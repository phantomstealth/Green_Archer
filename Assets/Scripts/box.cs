using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour {
	public int NumSurprise;
	public GameObject prefabMoney;
    public GameObject prefabHearth;
    public GameObject prefabPrizeArrow;
    public GameObject prefabPrizeSword;
    public GameObject prefabPrizeShield;

	// Use this for initialization
	void Start () {
        if (NumSurprise == 0)
            NumSurprise = Random.Range(1, 4);
    }

    // Update is called once per frame
	public void GiveMeSurprise(){
		Vector3 newPos;
		newPos = transform.position;
        //newPos.z = 2;
        if (NumSurprise == 1) 	
			Instantiate (prefabMoney, newPos, transform.rotation);
        if (NumSurprise == 2)
            Instantiate(prefabHearth, newPos, transform.rotation);
        if (NumSurprise == 3)
            Instantiate(prefabPrizeArrow, newPos, transform.rotation);
        if (NumSurprise == 4)
            Instantiate(prefabPrizeSword, newPos, transform.rotation);
        if (NumSurprise == 5)
            Instantiate(prefabPrizeShield, newPos, transform.rotation);

    }
}
