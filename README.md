[![Build](https://github.com/DynamoDS/DynamoSamples/actions/workflows/build.yml/badge.svg)](https://github.com/DynamoDS/DynamoSamples/actions/workflows/build.yml)

![Image](https://raw.github.com/ikeough/Dynamo/master/doc/distrib/Images/dynamo_logo_dark.png)

# Dynamo Samples

A collection of samples demonstrating how to develop libraries for Dynamo.

These samples make use of the [Dynamo NuGet packages](https://www.nuget.org/packages?q=DynamoVisualProgramming). NuGet should take care of restoring these packages if they are not available on your system at build time.

# Building the Samples

## Requirements

- Visual Studio 2022
- .NET8

## Instructions

- Clone the repository.
- Choose a branch:
  - The master branch of Dynamo Samples corresponds to the master branch of Dynamo. To build against a specific version, choose that version's branch or tag. i.e. 0.8.0, 0.9.0, etc.
- Open `DynamoSamples.sln` with Visual Studio.
- Build using the `Debug/Any CPU` configuration.
- The `dynamo_package` folder at the root of the repository will now have the built libraries. The `Dynamo Samples` folder in that directory can be copied directly to your Dynamo packages directory:`C:\Users\<you>\AppData\Roaming\Dynamo Core\<version>\packages`.
- To install the sample view extension the `SampleViewExtension\bin\debug` folder (or release) should contain
  - `SampleViewExtension.dll` which should be copied to your root Dynamo build location
  - `SampleViewExtension_ViewExtensionDefinition` which should be copied to the `viewExtensions` folder inside your root Dynamo build location
- Run Dynamo. You should find `SampleLibraryUI` and `SampleLibraryZeroTouch` categories in your library and the `View` tab inside of Dynamo should now contain `Show View Extension Sample Window`.
