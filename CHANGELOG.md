# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.4.0] - 2024-10-29
### Deprecated
- Deprecated synchronous `Blob.Create` and `File.Create` creator methods in favor of asynchronous `CreateAsync` methods.
- Deprecated `FileReader` event members `OnLoadStart`, `OnProgress`, `OnLoad`, `OnAbort`, and `OnLoadEnd` in favor of methods for adding and removing event listeners like `AddOnLoadEventListenerAsync` and `RemoveOnLoadEventListenerAsync`.
- Deprecated `FileReaderInProcess` event members `OnLoadStart`, `OnProgress`, `OnLoad`, `OnAbort`, and `OnLoadEnd` in favor of methods for adding and removing event listeners like `AddOnLoadEventListener` and `RemoveOnLoadEventListener`. 
### Changed
- Changed the version of Blazor.Streams to use the newest version which is 0.5.0.
- `FileReader` now extends `EventTarget` to expose all methods for adding/removing event listeners and dispatching events.
- `FileReaderInProcess` now implements `IEventTargetInProcess` to expose all methods for adding/removing event listeners and dispatching events.
- `ProgressEvent` now extends `Event` to expose methods for getting general information about the event like the target, type, and time stamp and asynchronous methods for stopping propagation and preventing default behavior programmatically.
- `ProgressEventInProcess` now exposes same members as `EventInProcess`. This includes properties for getting general information about the event like the target, type, and time stamp and synchronous methods for stopping propagation and preventing default behavior programmatically.
### Added
- Added target for .NET 8.
- Added `CreateAsync` creator methods for all wrapper classes.
- Added `IAsyncDisposable` implementation to all wrapper classes that ensures that their helpers and `JSReference`s are disposed.

## [0.3.0] - 2023-03-16
### Changed
- Changed .NET version to `7.0`.
- Changed the version of Blazor.Streams to use the newest version which is `0.3.0`.
### Added
- Added the generation of a documentation file packaging all XML comments with the package.

## [0.2.0] - 2022-11-18
### Added
- Added implicit converters for `BlobPart` from `Blob`, `byte[]`, and `string`.
### Fixed
- Fixed that the saturating creator for `FileReader` didn't register event handlers.

## [0.1.1] - 2022-11-10
### Fixed
- `Blob`'s method `ArrayBufferAsync` used an `IJSInProcessObjectReference` which is not supported in Blazor Server. This was changed to use `IJSObjectReference`.
### Changed
- Changed the version of Blazor.Streams to use the newest version which is `0.2.2`.

## [0.1.0] - 2022-11-08
### Added
- Made the initial implementation that covers most of the API.