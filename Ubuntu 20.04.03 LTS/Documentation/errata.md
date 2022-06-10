# Errata *Pro ASP.NET Core 6 9th Edition*

## General

Relative links from the current directory e.g. `href="/lib/bootstrap/...` when used throughout the `.html` listings
often begin with `/` which can sometimes be interpreted as a root path. Instead it seems better to begin with current
directory explicitly e.g. `href="./lib/bootstrap/...`. This is an *edge case* but as an example it is necessary when
using VS Code *Follow Link* feature on Ubuntu.

## Ch4

### On Page 67:

> The new browser window can be displayed by setting the `launchBrowser` property shown in Listing 4-5 to *false*,
> but you will have to perform a manual reload the first time you start or restart ASP.NET Core.

Suggest clarification:

> The new browser window can also be displayed when the `launchBrowser` property shown in Listing 4-5 is set to
> *false*, but you will have to perform a manual load or reload the first time you start or restart ASP.NET Core.

### On Page 68:

> If you are using Visual Studio, add a file named MyClass.cs to the MyProject folder with the content shown in
> Listing 4-10.

Should be:

> If you are using Visual Studio Code, add a file named MyClass.cs to the MyProject folder with the content shown in
> Listing 4-10.

### On Page 71:

> The dotnet tool install command installs version 3.1.1 of the dotnet-ef package, which is the version I use in this
> book.

Should be:

> The dotnet tool install command installs version 6.0.0 of the dotnet-ef package, which is the version I use in this
> book.

## Ch5

### On Page 83:

> A question mark (the ? character) is appended to a type to denote a nullable type. So, if a variable’s type is
> string?, for example, then it can be assigned any value string value or null. When attempting to access this
> variable, you should check to ensure that it isn’t null before attempting to access any of the fields, properties,
> or methods defined by the string type. But if a variable’s type is string, then it can be assigned
> null values, which means you can confidently access the features it provides without needing to guard
> against null references.

Should be:

> A question mark (the ? character) is appended to a type to denote a nullable type. So, if a variable’s type is
> string?, for example, then it can be assigned any string value or null. When attempting to access this
> variable, you should check to ensure that it isn’t null before attempting to access any of the fields, properties,
> or methods defined by the string type. But if a variable’s type is string, then it cannot be assigned
> null values, which means you can confidently access the features it provides without needing to guard
> against null references.

## Ch6

### On Page 128:

> * Tip The Fact attribute and the Asset class are defined in the Xunit namespace, for which there must be
> a using statement in every test class.

Suggest clarification:

> * Tip The Fact attribute and the Asset class are defined in the Xunit namespace, for which there must be
> a using statement in every test class. This is may be written with a `global using` statement in a single `Usings.cs` file (see Chapter 5 Understanding Global using Statements for details).

### On Page 134:

Suggest additional:

> For the `using Microsoft.AspNetCore.Mvc` statement to be valid it will be necessary to reference the [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing) package and add a reference to the ASP.Net Core framework in the `SimpleApp.Tests.csproj` file:
>
> ```xml
>   <ItemGroup>
>     <FrameworkReference Include="Microsoft.AspNetCore.App" />
>     <PackageReference Include="Microsoft.AspNetCore.MVC.Testing" Version="6.0.5" />    
>   </ItemGroup>
> ```

Further information can be found at:

* https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#test-app-prerequisites
* https://stackoverflow.com/questions/59726695/why-i-couldnt-use-microsoft-aspnetcore-mvc-in-my-unit-testing-project

## Ch7

On Page 142:

As per [Ch6](#on-page-134) add references for AspNetCore to `SportsSln/SportsStore.Tests/SportsStore.Tests.csproj` when creating the Unit Test Project:

> ```xml
>   <ItemGroup>
>     <FrameworkReference Include="Microsoft.AspNetCore.App" />
>     <PackageReference Include="Microsoft.AspNetCore.MVC.Testing" Version="6.0.5" />    
>   </ItemGroup>
> ```
