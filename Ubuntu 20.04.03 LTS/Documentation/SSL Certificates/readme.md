# SSL Certificates

--WORKING DOCUMENT SUBJECT TO CHANGE--

From details described in

* https://ubuntu.com/server/docs/security-certificates
* https://ubuntu.com/server/docs/security-trust-store
* [How to request custom certificates using the MMC snapin (PDF)](ssl/How%20to%20request%20custom%20certificates%20using%20the%20MMC%20snapin%20%E2%80%93%20Mister%20Cloud%20Tech.pdf)
* [Creating a trusted CA and SAN certificate using OpenSSL (PDF)](ssl/Ubuntu%20Creating%20a%20trusted%20CA%20and%20SAN%20certificate%20using%20OpenSSL%20%E2%80%93%20Fabian%20Lee%20_%20Software%20Engineer.pdf)
* [Example customized `openssl.cnf` file](ssl/server-name.cnf)

After following the basic setup described in the Ubuntu docs above, the process described in the PDFs was followed to ensure SAN certificates where generated (then tested with IIS10):

```bash
export prefix="server-name"
```

Note that `/usr/lib/ssl/openssl.cnf -> /etc/ssl/openssl.cnf`

```bash
sudo cp /etc/ssl/openssl.cnf /etc/ssl/$prefix.conf
sudo vi /etc/ssl/$prefix.cnf
```

See example customized `openssl.cnf` file and associated *PDF* instructions in links above for required changes. Then create CA certificate:

```bash
# Enter CN = server-name.full.domain.com (everything else default)
sudo openssl req -new -x509 -extensions v3_ca -keyout cakey.pem -out cacert.pem -days 3650 -config $prefix.cnf

sudo mv cacert.pem /etc/ssl/certs
sudo mv cakey.pem /etc/ssl/private
```

Sign the Certificate Signing Request (generated from IIS or another ubuntu installation) and copy the `cacert.pem` to a `.crt` file to install as a Trusted Root Certification Authority:

```bash
 sudo openssl ca -in ~/sambashare/$prefix.csr -extensions v3_req -config $prefix.cnf

 sudo cp /etc/ssl/newcerts/01.pem /etc/ssl/newcerts/$prefix.pem

 sudo openssl x509 -in /etc/ssl/newcerts$prefix.pem -inform PEM -outform DER
-out ~/sambashare/$prefix.cer

sudo cp /etc/ssl/certs/cacert.pem ~/sambashare/cacert.crt
 ```

For IIS install the certificate using "Complete Certificate Signing Request...".

For a development machine see also:

* [Enforce HTTPS in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-6.0&tabs=visual-studio)
* [Trust the ASP.NET Core HTTPS development certificate on Windows](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-6.0&tabs=visual-studio#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos)
* [Trust HTTPS certificate on Linux](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-6.0&tabs=visual-studio#trust-https-certificate-on-linux)
