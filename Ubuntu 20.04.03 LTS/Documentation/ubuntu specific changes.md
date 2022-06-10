# Ubuntu Specific Changes

## Ch7

Using dotnet runtime and dotnet-ef versions 6.0.5. dotnet sdk 6.0.300. Using mssql-express (see [Development Setup](./development%20setup.md#install-mssql-server-2019-express) for installation details)

### Page 153

Before creating the Database Migration, create file `/etc/profile.d/dotnet-environment-variables.sh` with the following content. The use of single quotes (`'`) surrounding the string allows for special characters such as `$` and `!` to be used in the password (some characters still have special meaning when using double quotes (`"`):

```text
export DOTNET_ConnectionStrings__SportsStoreConnection='Server=localhost;Database=SportsStore;User Id=USERNAME;Password=PASSWORD;MultipleActiveResultSets=true'
```

Followed by:

```bash
source /etc/profile.d/dotnet-environment-variables.sh
```

The connection string in `appsettings.json` can be left as an empty string. The default behaviour of Configuration Providers in .NET is that environment variables are read after file providers and the secret manager. They therefore override any values previously set in those locations.
