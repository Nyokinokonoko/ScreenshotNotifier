# ScreenshotNotifier

A native plugin detecting screenshot and screen recording activities for Unity. Built for mobile devices.

## Preview notice

- Only implementation for iOS devices (not including simulator) is ready
- On-device check only took place on iOS 18 devices
- Implementation and API specification may change over time

## Requirement

- Unity 2019+
- iOS 11+

### Tested on

- Unity 2022.3.50
- iOS 18.1

## Install

- Add the following git URL in Package Manager: `https://github.com/Nyokinokonoko/ScreenshotNotifier.git?path=UnityPackage/`

## Sample

```C#
using UnityEngine;
using KinokoForce.ScreenshotNotifier;

public class SampleUsage : MonoBehaviour
{
    private void Awake()
    {
        ScreenshotNotifier.OnScreenshotDetected += ScreenshotNotifierOnOnScreenshotDetected;
        ScreenshotNotifier.OnScreenRecordingStatusChanged += ScreenshotNotifierOnOnScreenRecordingStatusChanged;

        ScreenshotNotifier.EnterCaptureProhibitSession();
    }

    private void ScreenshotNotifierOnOnScreenRecordingStatusChanged(bool isrecording)
    {
        Debug.Log("ScreenshotNotifierOnOnScreenRecordingStatusChanged: " + isrecording);
    }

    private void ScreenshotNotifierOnOnScreenshotDetected()
    {
        Debug.Log("ScreenshotNotifierOnOnScreenshotDetected");
    }

    private void OnDestroy()
    {
        ScreenshotNotifier.ExitCaptureProhibitSession();
    }
}
```
