using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Pixelate")]
public class Pixelate : MonoBehaviour
{
	public Shader shader;
	int _pixelSizeX = 1;
	int _pixelSizeY = 1;
	Material _material;
	[Range(1, 20)] private int pixelSizeX = 1;
	[Range(1, 20)] private int pixelSizeY = 1;
	public bool lockXY = true;

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_material == null) _material = new Material(shader);
		_material.SetInt("_PixelateX", pixelSizeX);
		_material.SetInt("_PixelateY", pixelSizeY);
		Graphics.Blit(source, destination, _material);
	}

	void OnDisable()
	{
		DestroyImmediate(_material);
	}

	private int timeSt;

	void Start()
	{
		timeSt = 0;
		pixelSizeX = pixelSizeY = 4;
	}

	void Update()
	{
		timeSt++;
		if (timeSt == 40)
		{
			pixelSizeX = pixelSizeY = pixelSizeX + 1;
			timeSt = 0;
			if (pixelSizeX > 7)
			{
				pixelSizeX = pixelSizeY = 4;
			}
		}

		if (pixelSizeX != _pixelSizeX)
		{
			_pixelSizeX = pixelSizeX;
			if (lockXY) _pixelSizeY = pixelSizeY = _pixelSizeX;
		}

		if (pixelSizeY != _pixelSizeY)
		{
			_pixelSizeY = pixelSizeY;
			if (lockXY) _pixelSizeX = pixelSizeX = _pixelSizeY;
		}
	}
}