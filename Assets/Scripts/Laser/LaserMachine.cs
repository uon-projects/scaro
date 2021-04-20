using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lightbug.LaserMachine
{
	public class LaserMachine : MonoBehaviour
	{
		struct LaserElement
		{
			public Transform transform;
			public LineRenderer lineRenderer;
			public GameObject sparks;
			public bool impact;
		};

		private LaserElement _element;

		[Header("External Data")] [SerializeField]
		LaserData m_data;

		[Tooltip("This variable is true by default, all the inspector properties will be overridden.")] [SerializeField]
		bool m_overrideExternalProperties = true;

		[SerializeField] LaserProperties m_inspectorProperties = new LaserProperties();


		LaserProperties m_currentProperties; // = new LaserProperties();

		float m_time = 0;
		bool m_active = true;
		bool m_assignLaserMaterial;
		bool m_assignSparks;


		private void CreateLaserBeam()
		{
			m_currentProperties = m_overrideExternalProperties ? m_inspectorProperties : m_data.m_properties;

			m_currentProperties.m_initialTimingPhase = Mathf.Clamp01(m_currentProperties.m_initialTimingPhase);
			m_time = m_currentProperties.m_initialTimingPhase * m_currentProperties.m_intervalTime;

			m_assignSparks = m_data.m_laserSparks != null;
			m_assignLaserMaterial = m_data.m_laserMaterial != null;

			_element = new LaserElement();

			GameObject newObj = new GameObject("lineRenderer_1");

			if (m_currentProperties.m_physicsType == LaserProperties.PhysicsType.Physics2D)
				newObj.transform.position = (Vector2) transform.position;
			else
				newObj.transform.position = transform.position;

			newObj.transform.rotation = transform.rotation;
			newObj.transform.position += newObj.transform.forward * m_currentProperties.m_minRadialDistance;

			newObj.AddComponent<LineRenderer>();

			if (m_assignLaserMaterial)
				newObj.GetComponent<LineRenderer>().material = m_data.m_laserMaterial;

			newObj.GetComponent<LineRenderer>().receiveShadows = false;
			newObj.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			newObj.GetComponent<LineRenderer>().startWidth = m_currentProperties.m_rayWidth;
			newObj.GetComponent<LineRenderer>().useWorldSpace = true;
			newObj.GetComponent<LineRenderer>().SetPosition(0, newObj.transform.position);
			newObj.GetComponent<LineRenderer>().SetPosition(1,
				newObj.transform.position + transform.forward * m_currentProperties.m_maxRadialDistance);
			newObj.transform.SetParent(transform);

			if (m_assignSparks)
			{
				GameObject sparks = Instantiate(m_data.m_laserSparks);
				sparks.transform.SetParent(newObj.transform);
				sparks.SetActive(false);
				_element.sparks = sparks;
			}

			_element.transform = newObj.transform;
			_element.lineRenderer = newObj.GetComponent<LineRenderer>();
			_element.impact = false;
		}

		private void Start()
		{
			CreateLaserBeam();
		}

		private void Update()
		{
			RaycastHit hitInfo3D;

			if (m_active)
			{
				_element.lineRenderer.enabled = true;
				_element.lineRenderer.SetPosition(0, _element.transform.position);

				Physics.Linecast(
					_element.transform.position,
					_element.transform.position +
					_element.transform.forward * m_currentProperties.m_maxRadialDistance,
					out hitInfo3D,
					m_currentProperties.m_layerMask
				);


				if (hitInfo3D.collider)
				{
					_element.lineRenderer.SetPosition(1, hitInfo3D.point);

					if (m_assignSparks)
					{
						_element.sparks.transform.position =
							hitInfo3D.point; //new Vector3(rhit.point.x, rhit.point.y, transform.position.z);
						_element.sparks.transform.rotation = Quaternion.LookRotation(hitInfo3D.normal);
					}
				}
				else
				{
					_element.lineRenderer.SetPosition(1,
						_element.transform.position +
						_element.transform.forward * m_currentProperties.m_maxRadialDistance);
				}

				if (m_assignSparks)
					_element.sparks.SetActive(hitInfo3D.collider != null);
			}
			else
			{
				_element.lineRenderer.enabled = false;

				if (m_assignSparks)
					_element.sparks.SetActive(false);
			}
		}

		public void SetLaserActive(bool isActive)
		{
			m_active = isActive;
		}
	}
}