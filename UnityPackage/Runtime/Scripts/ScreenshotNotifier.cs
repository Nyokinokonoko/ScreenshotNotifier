using System;
using System.Runtime.InteropServices;

namespace KinokoForce.ScreenshotNotifier
{
    public static class ScreenshotNotifier
    {
        public static bool IsInCaptureProhibitSession { get; private set; } = false;

        public delegate void ScreenRecordingStatusChangedHandler(bool isRecording);
        public delegate void ScreenshotDetectedHandler();

        public static event ScreenRecordingStatusChangedHandler OnScreenRecordingStatusChanged;
        public static event ScreenshotDetectedHandler OnScreenshotDetected;
        
        public static void EnterCaptureProhibitSession()
        {
            if (IsInCaptureProhibitSession)
                return;

            NativeMethods.SetScreenRecordingStatusChangedCallback(OnNativeScreenRecordingStatusChanged);
            NativeMethods.SetScreenshotDetectedCallback(OnNativeScreenshotDetected);
            NativeMethods.EnterCaptureProhibitSession();

            IsInCaptureProhibitSession = true;
        }

        public static void ExitCaptureProhibitSession()
        {
            if (!IsInCaptureProhibitSession)
                return;

            NativeMethods.ExitCaptureProhibitSession();
            IsInCaptureProhibitSession = false;
        }

        public static bool GetCaptureStatus()
        {
            if (!IsInCaptureProhibitSession)
            {
                return false;
            }
            return NativeMethods.CheckCaptureStatus();
        }

        [AOT.MonoPInvokeCallback(typeof(ScreenRecordingStatusChangedHandler))]
        private static void OnNativeScreenRecordingStatusChanged(bool isRecording)
        {
            OnScreenRecordingStatusChanged?.Invoke(isRecording);
        }

        [AOT.MonoPInvokeCallback(typeof(ScreenshotDetectedHandler))]
        private static void OnNativeScreenshotDetected()
        {
            OnScreenshotDetected?.Invoke();
        }

        private static class NativeMethods
        {
            // Import native methods
            [DllImport("__Internal")]
            internal static extern void SetScreenRecordingStatusChangedCallback(ScreenRecordingStatusChangedHandler callback);

            [DllImport("__Internal")]
            internal static extern void SetScreenshotDetectedCallback(ScreenshotDetectedHandler callback);

            [DllImport("__Internal")]
            internal static extern void EnterCaptureProhibitSession();

            [DllImport("__Internal")]
            internal static extern void ExitCaptureProhibitSession();

            [DllImport("__Internal")]
            internal static extern bool CheckCaptureStatus();
        }
    }
}
