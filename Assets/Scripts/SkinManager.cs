using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [SerializeField] private Texture[] _textures;
    private Material _mat;

    private void Awake()
    {
        _mat = GetComponent<Renderer>().material;
    }
    private void OnEnable()
    {
        _mat.SetTexture("_Texture", _textures[PlayerDataManager.Instance.ActiveSkin]);
    }

    public void SetTexture(int textureIndex)
    {
        _mat.SetTexture("_Texture", _textures[textureIndex]);
    }
}
