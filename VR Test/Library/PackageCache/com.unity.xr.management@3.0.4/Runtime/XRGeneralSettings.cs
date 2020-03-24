using System;
using System.Collections;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.XR.Management
{
    /// <summary>General settings container used to house the instance of the active settings as well as the manager
    /// instance used to load the loaders with.
    /// </summary>
    public class XRGeneralSettings : ScriptableObject
    {
        /// <summary>The key used to query to get the current loader settings.</summary>
        public static string k_SettingsKey = "com.unity.xr.management.loader_settings";
        internal static XRGeneralSettings s_RuntimeSettingsInstance = null;

        [SerializeField]
        internal XRManagerSettings m_LoaderManagerInstance = null;

        [SerializeField]
        internal bool m_InitManagerOnStart = true;

        /// <summary>The current active manager used to manage XR lifetime.</summary>
        public XRManagerSettings Manager
        {
            get { return m_LoaderManagerInstance; }
            set { m_LoaderManagerInstance = value; }
        }

        private XRManagerSettings m_XRManager = null;

        private bool m_ProviderIntialized = false;
        private bool m_ProviderStarted = false;

        /// <summary>The current settings instance.</summary>
        public static XRGeneralSettings Instance
        {
            get
            {
                return s_RuntimeSettingsInstance;
            }
#if UNITY_EDITOR
            set
            {
                s_RuntimeSettingsInstance = value;
            }
#endif
        }

        /// <summary>The current active manager used to manage XR lifetime.</summary>
        public XRManagerSettings AssignedSettings
        {
            get
            {
                return m_LoaderManagerInstance;
            }
#if UNITY_EDITOR
            set
            {
                m_LoaderManagerInstance = value;
            }
#endif
        }

        /// <summary>Used to set if the manager is activated and initialized on startup.</summary>
        public bool InitManagerOnStart
        {
            get
            {
                return m_InitManagerOnStart;
            }
            #if UNITY_EDITOR
            set
            {
                m_InitManagerOnStart = value;
            }
            #endif
        }


#if !UNITY_EDITOR
        void Awake()
        {
            Debug.Log("XRGeneral Settings awakening...");
            s_RuntimeSettingsInstance = this;
            Application.quitting += Quit;
            DontDestroyOnLoad(s_RuntimeSettingsInstance);
        }
#endif

#if UNITY_EDITOR

        void Pause()
        {
            if (m_ProviderIntialized && m_ProviderStarted)
            {
                StopXRSDK();
            }
        }

        void Unpause()
        {
            if (m_ProviderIntialized && !m_ProviderStarted)
            {
                StartXRSDK();
            }
        }

        public void InternalPauseStateChanged(PauseState state)
        {
            switch (state)
            {
                case PauseState.Paused:
                    Pause();
                    break;
                case PauseState.Unpaused:
                    Unpause();
                    break;
            }
        }

        /// <summary>For internal use only.</summary>
        public void InternalPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingPlayMode:
                    Quit();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                case PlayModeStateChange.EnteredPlayMode:
                case PlayModeStateChange.EnteredEditMode:
                    break;
            }
        }
#endif
        static void Quit()
        {
            XRGeneralSettings instance = XRGeneralSettings.Instance;
            if (instance == null)
                return;

            instance.OnDisable();
            instance.OnDestroy();
        }

        void Start()
        {
            StartXRSDK();
        }

        void OnDisable()
        {
            StopXRSDK();
        }

        void OnDestroy()
        {
            DeInitXRSDK();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void AttemptInitializeXRSDKOnLoad()
        {
            XRGeneralSettings instance = XRGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.InitXRSDK();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        internal static void AttemptStartXRSDKOnBeforeSplashScreen()
        {
            XRGeneralSettings instance = XRGeneralSettings.Instance;
            if (instance == null || !instance.InitManagerOnStart)
                return;

            instance.StartXRSDK();
        }

        private void InitXRSDK()
        {
            if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.m_LoaderManagerInstance == null || XRGeneralSettings.Instance.m_InitManagerOnStart == false)
                return;

            m_XRManager = XRGeneralSettings.Instance.m_LoaderManagerInstance;
            if (m_XRManager == null)
            {
                Debug.LogError("Assigned GameObject for XR Management loading is invalid. XR SDK will not be automatically loaded.");
                return;
            }

            m_XRManager.automaticLoading = false;
            m_XRManager.automaticRunning = false;
            m_XRManager.InitializeLoaderSync();
            m_ProviderIntialized = true;
        }

        private void StartXRSDK()
        {
            if (m_XRManager != null && m_XRManager.activeLoader != null)
            {
                m_XRManager.StartSubsystems();
                m_ProviderStarted = true;
            }
        }

        private void StopXRSDK()
        {
            if (m_XRManager != null && m_XRManager.activeLoader != null)
            {
                m_XRManager.StopSubsystems();
                m_ProviderStarted = false;
            }
        }

        private void DeInitXRSDK()
        {
            if (m_XRManager != null && m_XRManager.activeLoader != null)
            {
                m_XRManager.DeinitializeLoader();
                m_XRManager = null;
                m_ProviderIntialized = false;
            }
        }

    }
}
