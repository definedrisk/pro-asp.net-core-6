# Errata *Pro ASP.NET Core 6 9th Edition*

## General

Relative links from the current directory e.g. `href="/lib/bootstrap/...` when used throughout the `.html` listings often begin with `/` which can sometimes be interpreted as a root path. Instead it seems better to begin with current directory explicitly e.g. `href="./lib/bootstrap/...`. This is an *edge case* but as an example it is necessary when using VS Code *Follow Link* feature on Ubuntu.

## Ch4

On Page 67:

> The new browser window can be displayed by setting the `launchBrowser` property shown in Listing 4-5 to *false*,
> but you will have to perform a manual reload the first time you start or restart ASP.NET Core.

Suggest clarification:

> The new browser window can also be displayed when the `launchBrowser` property shown in Listing 4-5 is set to
> *false*, but you will have to perform a manual load or reload the first time you start or restart ASP.NET Core.

On Page 68:

> If you are using Visual Studio, add a file named MyClass.cs to the MyProject folder with the content shown in
> Listing 4-10.

Should be:

> If you are using Visual Studio Code, add a file named MyClass.cs to the MyProject folder with the content shown in
> Listing 4-10.

On Page 71:

> The dotnet tool install command installs version 3.1.1 of the dotnet-ef package, which is the version I use in this
> book.

Should be:

> The dotnet tool install command installs version 6.0.0 of the dotnet-ef package, which is the version I use in this
> book.
