# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.1] - 2022-11-10
### Fixed
- `Blob`'s method `ArrayBufferAsync` used an `IJSInProcessObjectReference` which is not supported in Blazor Server. This was changed to use `IJSObjectReference`.
### Changed
- Changed the version of Blazor.Streams to use the newest version which is `0.2.2`.

## [0.1.0] - 2022-11-08
### Added
- Made the initial implementation that covers most of the API.