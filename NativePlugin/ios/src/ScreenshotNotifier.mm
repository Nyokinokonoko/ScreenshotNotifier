//
//  ScreenshotNotifier.mm
//  ScreenshotNotifier
//
//  Created by Kenny Ha on 2024/11/05.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

// Define function pointers for callbacks
typedef void (*ScreenRecordingStatusChangedCallback)(bool);
typedef void (*ScreenshotDetectedCallback)();

// Global variables for callbacks
static ScreenRecordingStatusChangedCallback screenRecordingStatusChangedCallback = NULL;
static ScreenshotDetectedCallback screenshotDetectedCallback = NULL;

// Observers
static id screenRecordingObserver;
static id screenshotObserver;

extern "C" {

// Set up the callback for when the screen recording status changes
void SetScreenRecordingStatusChangedCallback(ScreenRecordingStatusChangedCallback callback) {
    screenRecordingStatusChangedCallback = callback;
}

// Set up the callback for when a screenshot is detected
void SetScreenshotDetectedCallback(ScreenshotDetectedCallback callback) {
    screenshotDetectedCallback = callback;
}

// Enter capture prohibit session: set up observers
void EnterCaptureProhibitSession() {
    // Check and notify the initial screen recording status
    bool isRecording = [UIScreen mainScreen].isCaptured;
    if (screenRecordingStatusChangedCallback) {
        screenRecordingStatusChangedCallback(isRecording);
    }

    // Register for UIScreen capture state changes
    screenRecordingObserver = [[NSNotificationCenter defaultCenter] addObserverForName:UIScreenCapturedDidChangeNotification object:nil queue:[NSOperationQueue mainQueue] usingBlock:^(NSNotification * _Nonnull note) {
        bool isCaptured = [UIScreen mainScreen].isCaptured;
        if (screenRecordingStatusChangedCallback) {
            screenRecordingStatusChangedCallback(isCaptured);
        }
    }];

    // Register for screenshot detection
    screenshotObserver = [[NSNotificationCenter defaultCenter] addObserverForName:UIApplicationUserDidTakeScreenshotNotification object:nil queue:[NSOperationQueue mainQueue] usingBlock:^(NSNotification * _Nonnull note) {
        if (screenshotDetectedCallback) {
            screenshotDetectedCallback();
        }
    }];
}

// Exit capture prohibit session: remove observers
void ExitCaptureProhibitSession() {
    if (screenRecordingObserver) {
        [[NSNotificationCenter defaultCenter] removeObserver:screenRecordingObserver];
        screenRecordingObserver = nil;
    }

    if (screenshotObserver) {
        [[NSNotificationCenter defaultCenter] removeObserver:screenshotObserver];
        screenshotObserver = nil;
    }
}

// Check capture status directly
bool CheckCaptureStatus() {
    return [UIScreen mainScreen].isCaptured;
}

}
