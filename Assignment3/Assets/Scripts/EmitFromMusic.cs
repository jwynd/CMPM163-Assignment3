using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitFromMusic : MonoBehaviour
{
    enum LorF{
        Fireworks,
        Lightning
    }
    private ParticleSystem ps;
    [SerializeField][Range(0.6f, 0.8f)] private float magCutoff = 0.6f;
    [SerializeField] private LorF sys = LorF.Fireworks;
    // Start is called before the first frame update
    void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
        ps.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
        int numPartitions = 1;
        float[] aveMag = new float[numPartitions];
        float partitionIndx = 0;
        int numDisplayedBins = 512 / 2; 

        for (int i = 0; i < numDisplayedBins; i++) 
        {
            if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions){
                aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
            }
            else{
                partitionIndx++;
                i--;
            }
        }

        for(int i = 0; i < numPartitions; i++)
        {
            aveMag[i] = (float)0.5 + aveMag[i]*100;
            if (aveMag[i] > 100) {
                aveMag[i] = 100;
            }
        }

        float mag = aveMag[0];
        
        
        if(Input.GetKey(KeyCode.LeftArrow) && sys == LorF.Fireworks){
            magCutoff -= Time.deltaTime/20;
        }else if(Input.GetKey(KeyCode.RightArrow) && sys == LorF.Fireworks){
            magCutoff += Time.deltaTime/20;
        }
        if(Input.GetKey(KeyCode.UpArrow) && sys == LorF.Lightning){
            magCutoff += Time.deltaTime/20;
        } else if(Input.GetKey(KeyCode.DownArrow) && sys == LorF.Lightning){
            magCutoff -= Time.deltaTime/20;
        }
        magCutoff = Mathf.Clamp(magCutoff, 0.6f, 0.8f);
        if(mag > magCutoff){
            ps.Emit(Mathf.RoundToInt(mag));
        }
    }
}
