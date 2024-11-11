using System;
using System.Runtime.InteropServices;

namespace KinokoForce.ScreenshotNotifier
{
    /// <summary>
    /// Provides functionality to detect and handle screen recording and screenshot events. (Currently only works on iOS devices)
    /// </summary>
    public static class ScreenshotNotifier
    {
        /// <summary>
        /// Indicates whether the application is in a session where screen capture is prohibited.
        /// </summary>
        public static bool IsInCaptureProhibitSession { get; private set; } = false;

        /// <summary>
        /// Delegate for handling changes in screen recording status.
        /// </summary>
        /// <param name="isRecording">Indicates if screen recording is currently active.</param>
        public delegate void ScreenRecordingStatusChangedHandler(bool isRecording);

        /// <summary>
        /// Delegate for handling screenshot detection events.
        /// </summary>
        public delegate void ScreenshotDetectedHandler();

        /// <summary>
        /// Event triggered when the screen recording status changes.
        /// </summary>
        public static event ScreenRecordingStatusChangedHandler OnScreenRecordingStatusChanged;

        /// <summary>
        /// Event triggered when a screenshot is detected.
        /// </summary>
        public static event ScreenshotDetectedHandler OnScreenshotDetected;
        
        /// <summary>
        /// Enters a session where screen capture is prohibited. Sets up event callbacks for screen recording and screenshot events.
        /// </summary>
        public static void EnterCaptureProhibitSession()
        {
            if (IsInCaptureProhibitSession)
                return;

            NativeMethods.SetScreenRecordingStatusChangedCallback(OnNativeScreenRecordingStatusChanged);
            NativeMethods.SetScreenshotDetectedCallback(OnNativeScreenshotDetected);
            NativeMethods.EnterCaptureProhibitSession();

            IsInCaptureProhibitSession = true;
        }

        /// <summary>
        /// Exits the capture-prohibited session and removes event callbacks.
        /// </summary>
        public static void ExitCaptureProhibitSession()
        {
            if (!IsInCaptureProhibitSession)
                return;

            NativeMethods.SetScreenRecordingStatusChangedCallback(null);
            NativeMethods.SetScreenshotDetectedCallback(null);

            NativeMethods.ExitCaptureProhibitSession();
            IsInCaptureProhibitSession = false;
        }

        /// <summary>
        /// Gets the current capture status. Returns <c>true</c> if capture is active within a capture-prohibited session, otherwise <c>false</c>.
        /// </summary>
        /// <returns>A boolean indicating the capture status.</returns>
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
