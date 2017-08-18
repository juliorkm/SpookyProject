using UnityEngine;
using System.Collections;

public class teste : MonoBehaviour {
    [SerializeField]
    private float[] dropRates;
    ulong[] dropped;

    void Start () {
        dropped = new ulong[dropRates.Length];
        for (int i = 0; i < dropped.Length; i++)
            dropped[i] = 0;
      
        for (ulong i = 0; i < 40000000; i++)
            spawn();

        for (int i = 0; i < dropped.Length; i++) 
            print("Chance: " + dropRates[i] + "; Real: " + (float) dropped[i] / 40000000);
            
    }

    void spawn()
    {
        float hit = Random.Range(0f, 1f);
        float aux = 0;
        for (int i = 0; i < dropRates.Length; i++)
        {
            aux += dropRates[i];
            if (aux >= hit)
            {
                dropped[i]++;
                break;
            }
        }
    }
	
	
}
