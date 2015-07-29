[![Build status](https://ci.appveyor.com/api/projects/status/qjdj92r86xb2tbq3?svg=true)](https://ci.appveyor.com/project/ikeough/dynamosamples)

![Image](https://raw.github.com/ikeough/Dynamo/master/doc/distrib/Images/dynamo_logo_dark.png)

# Dynamo Samples
A collection of samples demonstrating how to develop libraries for Dynamo.

These samples make use of the Dynamo NuGet packages. NuGet should take care of restoring these packages if they are not available on your system at build time. 

**NOTE: This samples library is a work in progress. It will become fully functional with Dynamo 0.8.2.**

# Building the Samples

- Clone the repository.
- In VisualStudio 2013 or greater, open DynamoSamples.2013.sln.
- Build the Debug/Any CPU configuration.
- The `dynamo_package` folder at the root of the repository will now have the built libraries. The `Dynamo Samples` folder in that directory can be copied directly to your Dynamo packages directory:`C:\Users\<you>\AppData\Roaming\Dynamo\0.8\packages`.
- Run Dynamo. You should find SampleLibraryUI and SampleLibraryZeroTouch categories in your library.

