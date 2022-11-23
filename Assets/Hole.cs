using UnityEngine;

public class Hole : MonoBehaviour, IDamageable
{
    [SerializeField]
    private ComputeShader shader;

    [SerializeField]
    private int textSize = 256;

    [SerializeField]
    private int holeSize = 8;

    [SerializeField]
    private RenderTexture renderTexture;

    // Stored required properties.
    private int kernel;
    private Texture2D holeTexure;

    private void Awake()
    {
        kernel = shader.FindKernel("CSMain");

        renderTexture = new RenderTexture(textSize, textSize, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        shader.SetTexture(kernel, "Result", renderTexture);

        //holeTexure = new Texture2D(textSize, textSize);

        //holeTexure.filterMode = FilterMode.Bilinear;
        //holeTexure.wrapMode = TextureWrapMode.Clamp;

        //for (int i = 0; i < textSize; i++)
        //{
        //    for (int j = 0; j < textSize; j++)
        //    {
        //        holeTexure.SetPixel(i, j, Color.black);
        //    }
        //}

        //holeTexure.Apply();

        GetComponent<MeshRenderer>().material.SetTexture("_Hole", renderTexture);
    }

    public void Damage(Vector3 point, float radius)
    {

    }

    public void Damage(RaycastHit hitInfo)
    {
        Vector2 pos = hitInfo.textureCoord * textSize;

        RunShader((int)pos.x, (int)pos.y);

        //int size = holeMask.width;
        //int halfSize = size / 2;

        //holeTexure.SetPixels((int)pos.x - halfSize, (int)pos.y - halfSize, size, size, holeMask.GetPixels());

        //for (int i = 0; i < holeSize; i++)
        //{
        //    for (int j = 0; j < holeSize; j++)
        //    {
        //        int x = (int)(pos.x + i - halfHoleSize);
        //        int y = (int)(pos.y + j - halfHoleSize);

        //        float value = Mathf.Clamp01(1 - Vector2.Distance(new Vector2(x, y), pos) / halfHoleSize);

        //        value *= value;

        //        float lastValue = holeTexure.GetPixel(x, y).r;

        //        holeTexure.SetPixel(x, y, new Color(lastValue + value, 0, 0));
        //    }
        //}

        //holeTexure.Apply();
    }

    void RunShader(int x, int y)
    {
        shader.SetInt("positionX", x);
        shader.SetInt("positionY", y);
        shader.SetInt("size", holeSize / 2);
        shader.Dispatch(kernel, textSize / 8, textSize / 8, 1);
    }
}
