# SharpOMatic Editor Hosting

This package builds the Angular editor and embeds the static assets for ASP.NET Core hosting.

## Usage

```csharp
app.MapSharpOMaticEditor("/editor");
```

## Build

`dotnet build` or `dotnet pack` runs the Angular production build and embeds the output.
