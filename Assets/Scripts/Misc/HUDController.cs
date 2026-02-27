using UnityEngine;

public class HUDController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("The fill portion of the HP (health) bar")]
    private RectTransform m_HPBar;

    [SerializeField]
    [Tooltip("The fill portion of the MP (stamina) bar")]
    private RectTransform m_MPBar;
    #endregion

    #region Private Variables
    private float p_HPBarOrigWidth;
    private float p_MPBarOrigWidth;
    #endregion

    #region Initialization
    private void Awake()
    {
        if (m_HPBar != null)
            p_HPBarOrigWidth = m_HPBar.rect.width;

        if (m_MPBar != null)
            p_MPBarOrigWidth = m_MPBar.rect.width;
    }
    #endregion

    #region Public Update Methods

    public void UpdateHP(float percent)
    {
        if (m_HPBar == null) return;
        percent = Mathf.Clamp01(percent);
        m_HPBar.sizeDelta = new Vector2(150 * percent, m_HPBar.sizeDelta.y);
    }

    public void UpdateMP(float percent)
    {
        if (m_MPBar == null) return;
        percent = Mathf.Clamp01(percent);
        m_MPBar.sizeDelta = new Vector2(150 * percent, m_MPBar.sizeDelta.y);
    }

    #endregion
}