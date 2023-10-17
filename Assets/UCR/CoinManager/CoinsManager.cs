using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CoinsManager : MonoBehaviour
{
    public PathCreator pathCreator;
    public GameObject coinObject;

    public int[] coinCounting;
    public float numCoins = 1000;
    public static CoinsManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start() {
        coinCounting = new int[] { 0, 0};

        for (float i = 0; i < 1; i += (1/numCoins))
        {
            GameObject coinElement = Instantiate(coinObject);
            coinElement.transform.position = pathCreator.path.GetPointAtTime(i);
        }
    }
}
