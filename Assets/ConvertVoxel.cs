using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertVoxel : MonoBehaviour
{
  public GameObject box;
  public float size;
  public float collisionSize;
  public bool useObjectTexture;
  public Color boxColor;
  Renderer modelRenderer;
  Texture2D texture;
  void Start()
  {
    modelRenderer = GetComponentInChildren<Renderer>();
    texture = (Texture2D)modelRenderer.material.mainTexture;
    Voxel();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      GameObject[] cubePixels = GameObject.FindGameObjectsWithTag("CubePixel");
      foreach (GameObject cube in cubePixels)
      {
        BoxCollider bc = cube.GetComponent<BoxCollider>();
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (bc != null && rb != null)
        {
          bc.enabled = true;
          rb.isKinematic = false;
        }
      }
    }
  }

  void Voxel()
  {
    box.transform.localScale = new Vector3(size, size, size);
    for (float y = 0; y < 200; y += size)
    {
      for (float x = -50; x < 50; x += size)
      {
        for (float z = -50; z < 50; z += size)
        {
          Vector3 pos = new Vector3(x, y, z);
          if (Physics.CheckSphere(pos, collisionSize))
          {
            if (useObjectTexture)
            {
              // Converte a posição 'pos' de world para local
              Vector3 posLocal = modelRenderer.transform.InverseTransformPoint(pos);
              
              // Retorna o sólido envolvente (bounding box) do game object
              Vector3 objBounds = modelRenderer.bounds.min;

              // Obtem as coordenadas da textura na posição de 'pos'
              Vector2 textureCoord = modelRenderer.material.mainTextureScale * (posLocal - objBounds);

              // Converte as coordenadas para inteiros
              int texX = Mathf.RoundToInt(texture.width * textureCoord.x);
              int texY = Mathf.RoundToInt(texture.height * textureCoord.y);

              // Garante que x e y estejam dentro dos limites da textura
              texX = Mathf.Clamp(texX, 0, texture.width - 1);
              texY = Mathf.Clamp(texY, 0, texture.height - 1);
              boxColor = texture.GetPixel(texX, texY);
            }
            // Instancia o objeto aplicando a cor
            GameObject boxInstance = Instantiate(box, pos, Quaternion.identity, this.transform);
            Renderer boxRenderer = boxInstance.GetComponent<Renderer>();
            boxRenderer.material.color = boxColor;
          }
        }
      }
    }
  }
}
