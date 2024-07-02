using UnityEngine;
using System.Collections;

namespace TLab.Android.WebView
{
    public class TLabWebViewSample : MonoBehaviour
    {
        [SerializeField] private TLabWebView m_webView;
        [SerializeField] private float m_delayStartSeconds = 1.0f;

        bool started = false;

        public void StartWebView()
        {
            m_webView.StartWebView();
        }

        IEnumerator DelayedStartWebView()
        {
            yield return new WaitForSeconds(m_delayStartSeconds);
            StartWebView();
            started = true;
        }

        void Start()
        {
            StartCoroutine(DelayedStartWebView());
        }

        void Update()
        {
#if UNITY_ANDROID
            if (started)
            {
                if (m_webView != null && m_webView.WebViewEnabled)
                {
                    m_webView.UpdateFrame();
                }
            }
#endif
        }
    }
}
