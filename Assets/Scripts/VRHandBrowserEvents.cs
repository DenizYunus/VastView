using TLab.Android.WebView;
using UnityEngine;

public class VRHandBrowserEvents : MonoBehaviour
{
    //private Vector3 firstContactPoint;
    private bool hasContact = false;

    public TLabWebView m_tlabWebView;
    public RectTransform m_webViewRect;

    private int m_lastXPos;
    private int m_lastYPos;
    private bool m_onTheWeb = false;
    //private const float m_rectZThreshold = 0.05f;

    private const int TOUCH_DOWN = 0;
    private const int TOUCH_UP = 1;
    private const int TOUCH_MOVE = 2;

    Collider col;

    public void SetProperties(TLabWebView webView, RectTransform webViewRect)
    {
        m_tlabWebView = webView;
        m_webViewRect = webViewRect;
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<HandPlayerController>().handShape == HandPlayerController.HandShape.Pointing)
        {
            hasContact = true;

            Vector3 invertPosition = m_webViewRect.transform.InverseTransformPoint(col.ClosestPoint(other.transform.position));

            // https://docs.unity3d.com/jp/2018.4/ScriptReference/Transform.InverseTransformPoint.html
            invertPosition.z *= m_webViewRect.transform.lossyScale.z;

            float uvX = invertPosition.x / m_webViewRect.rect.width + m_webViewRect.pivot.x;
            float uvY = 1.0f - (invertPosition.y / m_webViewRect.rect.height + m_webViewRect.pivot.x);

            m_onTheWeb = true;

            m_lastXPos = (int)(uvX * m_tlabWebView.WebWidth);
            m_lastYPos = (int)(uvY * m_tlabWebView.WebHeight);

            int eventNum = TOUCH_DOWN;

            m_tlabWebView.TouchEvent(m_lastXPos, m_lastYPos, eventNum);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasContact && GetComponent<HandPlayerController>().handShape == HandPlayerController.HandShape.Pointing)
        {
            Vector3 invertPosition = m_webViewRect.transform.InverseTransformPoint(col.ClosestPoint(other.transform.position));

            invertPosition.z *= m_webViewRect.transform.lossyScale.z;

            float uvX = invertPosition.x / m_webViewRect.rect.width + m_webViewRect.pivot.x;
            float uvY = 1.0f - (invertPosition.y / m_webViewRect.rect.height + m_webViewRect.pivot.x);

            m_onTheWeb = true;

            m_lastXPos = (int)(uvX * m_tlabWebView.WebWidth);
            m_lastYPos = (int)(uvY * m_tlabWebView.WebHeight);

            int eventNum = TOUCH_MOVE;

            m_tlabWebView.TouchEvent(m_lastXPos, m_lastYPos, eventNum);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasContact = false;

        m_tlabWebView.TouchEvent(m_lastXPos, m_lastYPos, TOUCH_UP);
    }
}