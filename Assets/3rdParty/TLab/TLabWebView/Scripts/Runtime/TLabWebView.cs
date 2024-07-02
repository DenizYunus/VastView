#define DEBUG
//#undef DEBUG

using System.Collections;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TLab.Android.WebView
{
    public class TLabWebView : MonoBehaviour
    {
        private enum DownloadOption
        {
            applicationFolder,
            downloadFolder
        }

        [SerializeField] private RawImage m_rawImage;
        [SerializeField] private string m_url = "https://youtube.com";

        [Header("File Download Settings")]
        [SerializeField] private DownloadOption m_dlOption;
        [SerializeField] private string m_subdir = "downloads";

        [Header("Resolution setting")]
        [SerializeField] private int m_webWidth = 1024;
        [SerializeField] private int m_webHeight = 1024;
        [SerializeField] private int m_texWidth = 512;
        [SerializeField] private int m_texHeight = 512;

        public int WebWidth { get => m_webWidth; }
        public int WebHeight { get => m_webHeight; }
        public int TexWidth { get => m_texWidth; }
        public int TexHeight { get => m_texHeight; }
        public bool WebViewEnabled
        {
            get => m_webViewEnable;
            set
            {
                m_webViewEnable = value && m_webViewInitialized;
            }
        }

        private bool m_webViewEnable = false;
        private bool m_webViewInitialized = false;
        private Texture2D m_webViewTexture;
        private Coroutine m_webvieStartTask;

        private IntPtr m_texId = IntPtr.Zero;

#if UNITY_ANDROID
        private static AndroidJavaClass m_NativeClass;
        private AndroidJavaObject m_NativePlugin;
#endif

        private delegate void RenderEventDelegate(int eventID);
        private static RenderEventDelegate RenderThreadHandle = new RenderEventDelegate(RunOnRenderThread);
        private static IntPtr RenderThreadHandlePtr = Marshal.GetFunctionPointerForDelegate(RenderThreadHandle);

        private static jvalue[] m_jniArgs;

        [AOT.MonoPInvokeCallback(typeof(RenderEventDelegate))]
        private static void RunOnRenderThread(int eventID)
        {
            AndroidJNI.AttachCurrentThread();

            IntPtr jniClass = AndroidJNI.FindClass("com/tlab/libwebview/UnityConnect");
            if (jniClass == IntPtr.Zero || jniClass == null)
            {
                Debug.Log($"jni class not found: {jniClass}");
                return;
            }

            IntPtr jniFunc = AndroidJNI.GetStaticMethodID(jniClass, "generateSharedTexture", "(II)V");
            if (jniFunc == IntPtr.Zero || jniFunc == null)
            {
                Debug.Log($"jni function not found !:{jniFunc} ");
                return;
            }

            AndroidJNI.CallStaticVoidMethod(jniClass, jniFunc, m_jniArgs);

            AndroidJNI.DetachCurrentThread();
        }

        public void Init(
            int webWidth, int webHeight,
            int tWidth, int tHeight,
            int sWidth, int sHeight,
            string url, int dlOption, string subDir)
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            if (m_NativePlugin != null)
            {
                m_NativePlugin.Call("initialize", webWidth, webHeight, tWidth, tHeight, sWidth, sHeight, url, dlOption, subDir);
            }
#endif
        }

        public bool IsInitialized()
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            if (m_NativePlugin != null)
            {
                return m_NativePlugin.Call<bool>("IsInitialized");
            }

            return false;
#else
            return false;
#endif
        }

        private void UpdateSurface()
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("updateSurface");
#endif
        }

        public byte[] GetWebTexturePixel()
        {
            if (!m_webViewEnable)
            {
                return new byte[0];
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            return (byte[])(Array)m_NativePlugin.Call<sbyte[]>("getPixel");
#else
            return new byte[0];
#endif
        }

        public IntPtr GetTexturePtr()
        {
            if (!m_webViewEnable)
            {
                return IntPtr.Zero;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            return (IntPtr)m_NativePlugin.Call<int>("getTexturePtr");
#else
            return IntPtr.Zero;
#endif
        }

        public string GetUrl()
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            return m_NativePlugin.Call<string>("getCurrentUrl");
#else
            return null;
#endif
        }

        public void CaptureHTMLSource()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("capturePage");
#endif
        }

        public void CaptureElementById(string id)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("captureElementById", id);
#endif
        }

        public string CurrentHTMLCaptured()
        {
            if (!m_webViewEnable)
            {
                return null;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            return m_NativePlugin.Call<string>("getCaptured");
#else
            return null;
#endif
        }

        public void CaptureUserAgent()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("captureUserAgent");
#endif
        }

        public string GetUserAgent()
        {
            if (!m_webViewEnable)
            {
                return "";
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            return m_NativePlugin.Call<string>("getUserAgent");
#else
            return "";
#endif
        }

        public void SetUserAgent(string ua, bool reload)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("setUserAgent", ua, reload);
#endif
        }

        public void LoadUrl(string url)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("loadUrl", url);
#endif
        }

        public void LoadHTML(string html, string baseURL)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("loadHtml", html, baseURL);
#endif
        }

        public void ZoomIn()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("zoomIn");
#endif
        }

        public void ZoomOut()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("zoomOut");
#endif
        }

        public void EvaluateJS(string js)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("evaluateJS", js);
#endif
        }

        public void GoForward()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("goForward");
#endif
        }

        public void GoBack()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("goBack");
#endif
        }

        public void TouchEvent(int x, int y, int eventNum)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("touchEvent", x, y, eventNum);
#endif
        }

        public void KeyEvent(char key)
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("keyEvent", key);
#endif
        }

        public void BackSpace()
        {
            if (!m_webViewEnable)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("backSpaceKey");
#endif
        }

        public void SetVisible(bool visible)
        {
            WebViewEnabled = visible;

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("setVisible", visible);
#endif
        }

        public void ClearCache(bool includeDiskFiles)
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("clearCash", includeDiskFiles);
#endif
        }

        public void ClearCookie()
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("clearCookie");
#endif
        }

        public void ClearHistory()
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            m_NativePlugin.Call("clearHistory");
#endif
        }

        public bool CheckForPermission(UnityEngine.Android.Permission permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            return UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission.ToString());
#else
            return false;
#endif
        }

        public void RequestPermission(UnityEngine.Android.Permission permission)
        {
#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            UnityEngine.Android.Permission.RequestUserPermission(permission.ToString());
#endif
        }

        private static void SetJNIParam(int[] args)
        {
            m_jniArgs = new jvalue[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                m_jniArgs[i] = new jvalue { i = args[i] };
            }
        }

        private IEnumerator StartWebViewTask()
        {
            Debug.Log("StartWebViewTask: Beginning coroutine");

            if (m_NativeClass == null)
            {
                Debug.Log("StartWebViewTask: Initializing m_NativeClass");
                m_NativeClass = new AndroidJavaClass("com.tlab.libwebview.UnityConnect");
                if (m_NativeClass == null)
                {
                    Debug.LogError("Failed to find UnityConnect class.");
                    yield break;
                }
            }

            yield return new WaitForEndOfFrame();

#if UNITY_ANDROID && !UNITY_EDITOR || DEBUG
            Debug.Log("StartWebViewTask: Creating m_NativePlugin instance");
            m_NativePlugin = new AndroidJavaObject("com.tlab.libwebview.UnityConnect");
            if (m_NativePlugin == null)
            {
                Debug.LogError("Failed to create UnityConnect instance.");
                yield break;
            }

            yield return new WaitForEndOfFrame();

            //Debug.Log("StartWebViewTask: Issuing plugin event");
            //CommandBuffer cmd = new CommandBuffer();
            //cmd.IssuePluginEvent(RenderThreadHandlePtr, 0);
            //Graphics.ExecuteCommandBuffer(cmd);
            
            //switch (SystemInfo.renderingThreadingMode)
            //{
            //    case UnityEngine.Rendering.RenderingThreadingMode.MultiThreaded:
            //        SetJNIParam(new int[] { m_texWidth, m_texHeight });
            //        GL.IssuePluginEvent(RenderThreadHandlePtr, 0);
            //        break;
            //    default:
                    m_NativePlugin.CallStatic("generateSharedTexture", m_texWidth, m_texHeight);
            //        break;
            //}

            yield return new WaitForEndOfFrame();

            Debug.Log("StartWebViewTask: Creating Texture2D");
            m_webViewTexture = new Texture2D(m_texWidth, m_texHeight, TextureFormat.ARGB32, false, false);
            m_webViewTexture.name = "WebImage";

            m_rawImage.texture = m_webViewTexture;

            Debug.Log("StartWebViewTask: Initializing plugin");
            Init(
                m_webWidth, m_webHeight,
                m_texWidth, m_texHeight,
                Screen.width, Screen.height,
                m_url, (int)m_dlOption, m_subdir);

            yield return new WaitForEndOfFrame();

            Debug.Log("StartWebViewTask: Waiting for initialization");
            while (!IsInitialized())
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("StartWebViewTask: WebView Initialized");
            yield return new WaitForEndOfFrame();

            m_webViewInitialized = true;
            m_webViewEnable = true;

            m_webvieStartTask = null;
#endif
        }

        public void StartWebView()
        {
            if (m_webvieStartTask == null && !m_webViewInitialized)
            {
                m_webvieStartTask = StartCoroutine(StartWebViewTask());
            }
        }

        public void UpdateFrame()
        {
            if (!m_webViewEnable)
            {
                return;
            }

            IntPtr texId = GetTexturePtr();
            if (m_texId != texId)
            {
                m_texId = texId;
                m_webViewTexture.UpdateExternalTexture(texId);
            }
        }

        public void Destroy()
        {
#if UNITY_ANDROID
            if (m_NativePlugin == null)
            {
                return;
            }

            m_NativePlugin.Call("Destroy");
            m_NativePlugin = null;
#endif
        }

        private void OnDestroy()
        {
            Destroy();
        }

        private void OnApplicationQuit()
        {
            Destroy();
        }
    }
}
