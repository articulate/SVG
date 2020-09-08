# SVG.NET

Public fork of the C# SVG rendering library on codeplex:
https://svg.codeplex.com/

This started out as a minor modification to enable the writing of proper SVG strings. But now after almost two years we have so many fixes and improvements that we decided to share our current codebase to the public in order to improve it even further.

So please feel free to fork it and open pull requests for any fix, improvement or feature you add.

Microsoft Public License:
https://svg.codeplex.com/license

It is also available via Nuget:

PM> Install-Package Svg

## Projects using the library
* [vvvv] (http://vvvv.org) a hybrid visual/textual live-programming environment for easy prototyping and development.
* [Posh] (https://github.com/vvvv/Posh) a windowing/interaction/drawing layer for c#/.net desktop applications with their GUI in a browser.
* [Timeliner] (https://github.com/vvvv/Timeliner) A Posh based timeline that can be controlled by and sends out its values via OSC.

## Building

To update the NuGet package that is consumed, do the following.

1. Make your desired changes.
2. Incremement the version in [appveyor.yml](./appveyor.yml#L1).
3. Issue a PR for your changes and get them reviewed, signed-off, and merged as usual.
4. Once the PR is merged, an AppVeyor build will be triggered which will produce the version of the package you specified in #2.
In the Desktop repo, update the references to the new version

## Building

To update the NuGet package that is consumed, do the following.

1. Make your desired changes.
2. Incremement the version in [appveyor.yml](./appveyor.yml#L1).
3. Issue a PR for your changes and get them reviewed, signed-off, and merged as usual.
4. Once the PR is merged, an AppVeyor build will be triggered which will produce the version of the package you specified in #2.
In the Desktop repo, update the references to the new version.