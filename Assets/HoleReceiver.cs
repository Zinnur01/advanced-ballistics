using UnityEngine;

public class HoleReceiver : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ComputeShader computeShader;

    [SerializeField]
    private Material material;

    [SerializeField]
    private int textSize = 256;

    [SerializeField]
    private int holeSize = 8;

    [SerializeField]
    private RenderTexture renderTexture;

    // Stored required components.
    //private Texture2D holeTexture;
    //private Texture2D holeMask;

    // Stored required properties.
    private ComputeShader shaderClone;
    private Material materialClone;
    private int kernel;

    private void Awake()
    {
        shaderClone = Instantiate(computeShader);

        materialClone = new Material(material);

        kernel = shaderClone.FindKernel("Compute");

        renderTexture = new RenderTexture(textSize, textSize, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        shaderClone.SetTexture(kernel, "Result", renderTexture);

        materialClone.SetTexture("_Hole", renderTexture);

        GetComponent<MeshRenderer>().sharedMaterial = materialClone;
    }

    #region [IDamageable Implementation]
    public void Damage(RaycastHit hitInfo)
    {
        Vector2 pos = hitInfo.textureCoord * textSize;
        GPUCompute((int)pos.x, (int)pos.y);
    }
    #endregion

    private void CPUCompute(int x, int y)
    {
        //int size = holeMask.width;
        //int halfSize = size / 2;

        //holeTexture.SetPixels(x - halfSize, y - halfSize, size, size, holeMask.GetPixels());

        //for (int i = 0; i < holeSize; i++)
        //{
        //    for (int j = 0; j < holeSize; j++)
        //    {
        //        int offsetX = (int)(x + i - halfSize);
        //        int offsetY = (int)(y + j - halfSize);

        //        float value = Mathf.Clamp01(1 - Vector2.Distance(new Vector2(x, y), new Vector2(offsetX, offsetY)) / halfSize);

        //        value *= value;

        //        float lastValue = holeTexture.GetPixel(offsetX, offsetY).r;
        //        holeTexture.SetPixel(x, y, new Color(lastValue + value, 0, 0));
        //    }
        //}

        //holeTexture.Apply();
    }

    private void GPUCompute(int x, int y)
    {
        shaderClone.SetInt("positionX", x);
        shaderClone.SetInt("positionY", y);
        shaderClone.SetInt("size", holeSize / 2);
        shaderClone.Dispatch(kernel, textSize / 8, textSize / 8, 1);
    }
}
