# GitHub Actions Workflows

This directory contains GitHub Actions workflow configurations that automate various CI/CD processes for the ScreenshotNotifier project.

## Workflows

### Build iOS Library (`buildlib_ios.yml`)

This workflow automates the building of the iOS native library for the ScreenshotNotifier Unity package.

**Triggers:**

- Push to `main` branch
- Pull requests targeting `main` branch
- Manual trigger (workflow_dispatch)

**What it does:**

1. Runs on macOS latest runner
2. Sets up Xcode development environment
3. Builds the iOS library using xcodebuild
4. Copies the built library (`libScreenshotNotifier.a`) to two locations:
   - `UnityPackage/Runtime/Plugins/iOS/`
   - `NativePlugin/ios/lib/`
5. Automatically commits and pushes the updated library files

**Note:** The workflow requires `GITHUB_TOKEN` secret for pushing changes back to the repository.
