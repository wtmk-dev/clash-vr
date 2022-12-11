using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

public class XROriginOverride : MonoBehaviour
{
    public void Update()
    {
        UpdateCharacterController();    
    }

    [SerializeField]
    private XROrigin m_XROrigin;
    [SerializeField]
    private CharacterController m_CharacterController;
    [SerializeField]
    private CharacterControllerDriver _Driver;

    /// <summary>
    /// Updates the <see cref="CharacterController.height"/> and <see cref="CharacterController.center"/>
    /// based on the camera's position.
    /// </summary>
    protected virtual void UpdateCharacterController()
    {
        if (m_XROrigin == null || m_CharacterController == null)
            return;

        var height = Mathf.Clamp(m_XROrigin.CameraInOriginSpaceHeight, _Driver.minHeight, _Driver.maxHeight);

        Vector3 center = m_XROrigin.CameraInOriginSpacePos;
        center.y = height / 2f + m_CharacterController.skinWidth;

        m_CharacterController.height = height;
        m_CharacterController.center = center;
    }
}
