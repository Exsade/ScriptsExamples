using UnityEngine;

//This script creating plane for shader effect. Plane will start from game object position to (_targetPosX, _targetPosY)

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RainbowRayMeshCreator : MonoBehaviour
{

    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _meshWidth = 1;
    [SerializeField] private float _targetPosX = 1;
    [SerializeField] private float _targetPosY = 1;

    [Range(0.0f, 1.0f)]
    public float EffectMask = 0;

    private int _yPolyCount = 1;
    private int _xPolyCount = 1;
    private float _startPosX = 0;
    private float _startPosY = 0;
    private Mesh _mesh;
    private Vector3[] _vertices;

    private MaterialPropertyBlock _propertyBlock;

    private static readonly int _effectMask = Shader.PropertyToID("_EffectMask");

    private float _time;
    private float _speedEffectMask = -1;

    private void OnEnable()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (EffectMask < 1 && _speedEffectMask > 0)
        {
            _time += Time.deltaTime;
            EffectMask = _time * _speedEffectMask;
            if (EffectMask > 1)
            {
                EffectMask = 1;
                _speedEffectMask = -1;
            }
        }

        _renderer.GetPropertyBlock(_propertyBlock);

        _propertyBlock.SetFloat(_effectMask, EffectMask);

        _renderer.SetPropertyBlock(_propertyBlock);
    }

    public void RunAnimationTarget(Vector3 target, float time)
    {
        _targetPosX = target.x;
        _targetPosY = target.y;
        _speedEffectMask = 1 / time;
        _time = 0;
        EffectMask = 0;
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Grid";

        Vector3 pos1 = new Vector3(_startPosX, _startPosY, 0);
        Vector3 pos2 = transform.InverseTransformPoint(new Vector3(_targetPosX, _targetPosY, 0));
        Vector3 offset = pos2 - pos1;
        Vector3 stepBetweenPositions = new Vector3((pos2.x - pos1.x) / _yPolyCount, (pos2.y - pos1.y) / _yPolyCount);
        Vector3 perpendicular = Vector3.Cross(offset, Vector3.forward).normalized;

        _vertices = new Vector3[(_xPolyCount + 1) * (_yPolyCount + 1)];
        Vector2[] uv = new Vector2[_vertices.Length];

        Vector3 currentPos = pos1;

        for (int i = 0, y = 0; y <= _yPolyCount; y++)
        {
            _vertices[i] = currentPos + perpendicular * _meshWidth;
            _vertices[i + 1] = currentPos - perpendicular * _meshWidth;

            uv[i] = new Vector2((float)0, (float)y / _yPolyCount);
            uv[i + 1] = new Vector2((float)1, (float)y / _yPolyCount);

            i += 2;
            currentPos += stepBetweenPositions;
        }

        _mesh.vertices = _vertices;
        _mesh.uv = uv;

        int[] triangles = new int[_xPolyCount * _yPolyCount * 6];
        for (int ti = 0, vi = 0, y = 0; y < _yPolyCount; y++, vi++)
        {
            for (int x = 0; x < _xPolyCount; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + _xPolyCount + 1;
                triangles[ti + 5] = vi + _xPolyCount + 2;
            }
        }
        _mesh.triangles = triangles;
    }
}