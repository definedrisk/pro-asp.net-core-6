# Host ASP.NET Core on Linux with Nginx

--WORKING DOCUMENT SUBJECT TO CHANGE--

[Micorosft Docs: Host ASP.NET Core on Linux with Nginx](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-5.0)

On the linux server in preparation (once only - not required for every project):

```bash
sudo mkdir /var/www/dotnet
sudo chown -R www-data:www-data /var/www/dotnet
sudo chmod g+s /var/www/dotnet
sudo chmod g+w /var/www/dotnet
```

Ensure user *\<username\>* has membership of the group `www-data` on the server:

```bash
sudo usermod -a -G www-data <username>
groups <username>
```

Remove HTTPS and configure app to run at the insecure endpoint only then add the Forwarded Headers Middleware:

```cs
using Microsoft.AspNetCore.HttpOverrides;

...

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
```

### Publish and copy over the app

Publish files to a folder on the local development machine:

```bash
dotnet publish -c Release -r ubuntu.20.04-x64 --self-contained false /p:EnvironmentName=Production
```

Use `rsync` to publish files after build, changing *group* ownership to `www-data` (it is not possible to change *user* to `www-data` using this method as `ssh-user` is not usually `root` on the server):

```bash
rsync --delete --chown=:www-data -avh ./bin/Release/net5.0/ubuntu.20.04-x64/publish/ user@server-name:/var/www/dotnet/MyNameSpace.API
```

Alternativley use Visual Studio with a publish profile to do same.

### Example setting Nginx Server Block on server

Note that environment variables (if required for Nginx itself) should be specified for nginx within `nginx-config` because nginx removes all environment variables inherited from its parent process as described in this [askubuntu discussion](https://askubuntu.com/questions/1299017/make-environment-variables-available-for-all-script-and-programming-language). In most cases environment variables are defined in the systemd service file settings.

Configuration for Nginx is done by adding files at `/etc/nginx/sites-available/my-new-site`

```bash
server {
        listen                  8080;
        listen                  8081 ssl;
        keepalive_timeout       60;
        server_name             server-name.full.domain.com;
        ssl_certificate         ssl/server-name.crt;
        ssl_certificate_key     ssl/server.key.secure;
        ssl_password_file       /var/lib/nginx/ssl_passwords.txt;
        location / {
                proxy_pass      http://127.0.0.1:5000;
                proxy_http_version      1.1;
                proxy_set_header        Upgrade $http_upgrade;
                proxy_set_header        Connection keep-alive;
                proxy_set_header        Host $host;
                proxy_cache_bypass      $http_upgrade
                proxy_set_header        X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header        X-Forwarded-Proto $scheme;
        }
}
```

Text passwords are stored line by line in `/var/lib/nginx/ssl_passwords.txt` therefore ensure only `root` has permissions:

```bash
sudo chmod 600 /var/lib/nginx/ssl_passwords.txt
```

Check Nginx configuration and force update:

```bash
sudo nginx -t
sudo nginx -s reload
```

Test the app by running directly on the server. Test access via Nginx.

### Use `systemd` to manage the process

Add a *service* file e.g. `/etc/systemd/system/kesterel-MyNameSpace.API.service`

Add environment variables under `[Service]` using `Environment="Foo=bar baz"` or follow the method described in https://serverfault.com/questions/413397/how-to-set-environment-variable-in-systemd-service when values should change between installations on different machines, where a seperate file can be added at `/etc/systemd/system/myservice.service.d/local.conf` which would contain *just* the `[Service] Environment="Foo=bar baz"` lines. Afterwards systemd merges the two files. Remember to `systemctl daemon-reload` if changing any of them. Another option (more secure) is to use `EnvironmentFile=` and point to a file that is only readable by the service account and root. This is better for secrets on a multi-user system when `systemctl show my_service` will show the whole configuration file to any user.

```bash
[Unit]
Description=MyNameSpace API

[Service]
WorkingDirectory=/var/www/dotnet/MyNameSpace.API
# For self contained app
ExecStart=/var/www/dotnet/MyNameSpace.API/MyNameSpace.API
# Alternative for framework dependent
#ExecStart=/usr/bin/dotnet /var/www/dotnet/MyNameSpace.API/MyNameSpace.API.dll
Restart=always

# Restart service after 60 seconds if the dotnet service crashes
RestartSec=60
KillSignal=SIGINT
SysLogIndentifier=dotnet-MyNameSpace.API
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
Environment=MyApp__DataSource__ConnectionString="server=server-name.full.domain.com;port=3306;uid=user;pwd=password;database=dbName"
Environment=MyApp__Static_Folder_Path=/var/www/dotnet/webroot/example/somewhere

[Install]
WantedBy=multi-user.target
```

Useful commands:

```bash
sudo systemctl daemon-reload
sudo systemctl enable kestrel-MyNameSpace.API.service
sudo systemctl start kestrel-MyNameSpace.API.service
sudo systemctl status kestrel-MyNameSpace.API.service
sudo journalctl -fu kestrel-MyNameSpace.API.service
cat /var/log/syslog | grep MyNameSpace.API
```
