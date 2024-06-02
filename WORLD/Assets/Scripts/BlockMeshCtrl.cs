using System.Collections;
using UnityEngine;

public class BlockMeshCtrl : MonoBehaviour
{
    public MeshRenderer[] MeshRenderer;
    public Texture2D[] Texture2D;
    public Texture2D[] DesTexture2D;
    Material material;
    public int j = 0;
    void Start()
    {
        MeshRenderer = GetComponentsInChildren<MeshRenderer>();
        StartCoroutine(DesState());
        SetPic();

    }
    public void SetPic()
    {
        MeshRenderer[0].material.mainTexture = Texture2D[0];
        MeshRenderer[1].material.mainTexture = Texture2D[1];
        for (int i = 2; i < MeshRenderer.Length; i++)
        {
            MeshRenderer[i].material.mainTexture = Texture2D[2];
        }
    }
    public IEnumerator DesState()
    {

        while (j < 10)
        {
            for (int i = 0; i < MeshRenderer.Length; i++)
            {
                material = MeshRenderer[i].material;

                material.SetTexture("_MetallicGlossMap", DesTexture2D[j]);
                //material.SetFloat("_Metallic", 1.0f
            }
            j++;
            yield return new WaitForSeconds(0.5f);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}