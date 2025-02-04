# Errata *Pro ASP.NET Core 6 9th Edition*

Where changes to code listings are given then the associated changes are also implicitly assumed to be required for the accompanying source code.

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

> * Tip The Fact attribute and the Assert class are defined in the Xunit namespace, for which there must be
> a using statement in every test class. This may be written with a `global using` statement in a single `Usings.cs` file (see Chapter 5 Understanding Global using Statements for details).

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

### On Page 142:

As per [Ch6](#on-page-134) it is necessary to add references for AspNetCore to `SportsSln/SportsStore.Tests/SportsStore.Tests.csproj` when creating the Unit Test Project:

> ```xml
>   <ItemGroup>
>     <FrameworkReference Include="Microsoft.AspNetCore.App" />
>     <PackageReference Include="Microsoft.AspNetCore.MVC.Testing" Version="6.0.5" />    
>   </ItemGroup>
> ```

### On Page 167:

Add to `HomeControllerTests.cs` :

```cs
...
using SportsStore.Models.ViewModels;
...
```

## Ch8

### On Page 183:

The *default* parameter value `productPage = 1` is unnecessary for the `Page{productPage:int}` routing schema. It is redundant and does not allow <http://localhost:5000/Page> to be accessed. It can therefore be removed:

With the routing schema ordering given <http://localhost:5000/Products/Page1> would actually be matched by `{category}/Page{productPage:int}`. This will result in the *category* parameter being assigned a string value "Products" (which is clearly not the intent).

The `MapDefaultControllerRoute()` matches <http://localhost:5000/> .

```cs
app.UseStaticFiles();

app.MapControllerRoute("catpage", "{category}/Page{productPage:int}",
    new { controller = "Home", action = "Index" });

app.MapControllerRoute("page", "Page{productPage:int}",
    new { controller = "Home", action = "Index" });

app.MapControllerRoute("category", "{category}",
    new { controller = "Home", action = "Index", productPage = 1 });

app.MapDefaultControllerRoute();
```

### On Page 183-185:

Using the above routing the functionality already occurs without the additional steps described here.

### On Page 192:

Typo:

`ViewViewComponentResult` should be `IViewComponentResult`.

### On Page 212

Missing *nullable* reference type `byte[] data = ...` should be `byte[]? data = ...`.

## Ch9

### On Page 221

For clarity, when *posting* the form to the */Cart* razor page `CartModel.OnPostRemove(long productId, string returnUrl)` I would change the case of the *name* attribute to match that of this receiving function (shown on Page 222) i.e using the same case as `returnUrl`. Although this functionality is case insensitive, it would help differentiate this particular form submisson from being directly associated with the `Product.ProductID` property and `Product` model:

```cs
 <input type="hidden" name="ProductID" value="@line.Product.ProductID" />
 ```

 To:

 ```cs
  <input type="hidden" name="productId" value="@line.Product.ProductID" />
  ```

Note that on page 201 the `asp-for="ProductID"` is necessary in this case due to the `@model Product`. This [input tag-helper](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0#the-input-tag-helper) creates the `id` and `name` HTML attributes according to the [expression name](https://docs.microsoft.com/en-US/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0#expression-names) obtained form the ModelState or Model i.e. it is the case-sensitive name of a property from the model.

### Page 224

For information: successfully using 6.1.1 of font awsome from [cdnjs](https://cdnjs.com/libraries/font-awesome).

### Page 235

For clarity I would change the case of the `orderID` property in the anonymous type to `OrderID`, matching that of the receiving property shown in Listing 9-22 on page 240 (although this functionality is case insensitive):

```cs
return RedirectToPage("/Completed", new { orderId = order.OrderID });
```
To:

```cs
return RedirectToPage("/Completed", new { OrderId = order.OrderID });
```
