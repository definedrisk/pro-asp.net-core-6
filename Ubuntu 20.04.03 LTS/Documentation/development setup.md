# Development Setup

The example development setup uses a virtual instance of Linux Ubuntu 20.04.4 (client) on Windows 10 (host) using [Oracle VM VirtualBox](https://www.virtualbox.org/)

This example is using free open source tools. It should also be possible to do this with [GitHub codespaces](https://github.com/features/codespaces) and the upcoming [Windows Dev Box](https://techcommunity.microsoft.com/t5/azure-developer-community-blog/introducing-microsoft-dev-box/ba-p/3412063) amongst other tool options.

## Virtual hardware settings and first boot

Original installation with Oracle VM Virtual Box 6.1.34 (QT 5.6.2) on Windows 10 Pro 19044.1706 Host.

Summary of VM settings prior to first boot (mostly defaults):
* 4096Mb memory
* Optical (for installation) and HD 40GB (dynamic on SSD)
* 128Mb Video memory, 1 monitor, VMSVGA (without 3D acceleration)
* Default audio and networking (NAT)
* Default serial (none available) and USB settings (USB 2.0)
* No shared folders
* Boot from CD image [ubuntu-20.04.4-desktop-amd64.iso](https://releases.ubuntu.com/20.04.4/ubuntu-20.04.4-desktop-amd64.iso)

Note: most of this document also applies to *-server* (without installing or using any GUI applications). When installing *-server* then select the option to install `openssh-server` and import a key during installation (this can be done by importing form GitHub).

### During first boot (install from ISO)

Select the following options:

* UK English
* Minimal installation
* Download while installing
* No third-party
* Default
* Link Ubuntu One and Microsoft accounts and activate live-patch

### During first run

Do the following:

* Use "Software & Updates" GUI
    * Ubunutu Software
        * select "Canonical-supported free and open-source (main)"
        * select "Community-supported free and open-source (universe)"
        * deselect restricted and multiverse
    * Updates
        * Subscribe to "Security updates only"
* Adjust resolution, sound and power settings as required
* Add user to `vboxsf` group
  
  This is necessary if using file shares via Oracle Virtual Box. It is usually necessary (easier) to restart the OS after changing group permissions so they update properly.
  
  ```code
  id
  groups
  getent group
  sudo usermod -a -G groupname username
  ```

## Working environment setup

Once the installation is complete and the system has been rebooted prepare the working environment by installing all the necessary applications and tools.

### Install *synaptic*

```bash
sudo apt install --install-suggests synaptic
```

### Confirm *python3* version

```bash
python3 -VV
```

### Install *pip3* and *venv*

```bash
sudo apt install -y python3-pip python3-venv
```

### Install *dconf-editor*

```bash
sudo apt install dconf-editor
```

Use *org > gnome > shell > extensions > dash-to-dock* and set `extend-height = false` and `dock-position = BOTTOM`.

### Install the Microsoft repositories

See [How To Add OpenPGP Repository Signing Keys](https://www.linuxuprising.com/2021/01/apt-key-is-deprecated-how-to-add.html) for a more secure way to add third-party (unoffical) repositories. 

Location https://packages.microsoft.com/config/ubuntu/20.04/ .

```bash
sudo apt install curl
curl -L https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor | sudo tee /usr/share/keyrings/microsoft-archive-keyring.gpg > /dev/null
```

Then create  `/etc/apt/sources.list.d/microsoft-prod.list` containing:

```text
deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/ubuntu/20.04/prod focal main
```

and `/etc/apt/sources.list.d/microsoft-mssql-server-2019.list` containing:

```text
deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/ubuntu/20.04/mssql-server-2019 focal main
```

Microsoft Edge browser is now [installed directly](#install-microsoft-edge-browser-and-gnome-extensions). This was the old method:
> and `/etc/apt/sources.list.d/microsoft-edge.list` containing:
> 
> ```text
> deb [arch=amd64 signed-by=/usr/share/keyrings/microsoft-archive-keyring.gpg] https://packages.microsoft.com/repos/edge stable main
> ```
> ```bash
> sudo apt update
> sudo apt install microsoft-edge-stable
> ```
### Install Microsoft Edge browser and Gnome Extensions

Install Edge by downloading package directly from the website: https://www.microsoft.com/en-us/edge . Alternatively follow instructions from https://www.microsoftedgeinsider.com/en-us/download/?platform=linux .

Then setup browser profile sync and log into github. Install *gnome-tweaks* extensions:

```bash
sudo apt install chrome-gnome-shell gnome-shell-extension-autohidetopbar
```

Use [Gnome Extensions](https://extensions.gnome.org) in Microsoft Edge browser to remove desktop icons (need to log out and back after install to ensure that the extensions are available). Find the *Hide Top Bar* extension under *Installed extensions* and use to make adjustments as required.

Set Edge as default browser.

### Install *openssh-server* (only if required)

Install `openssh-server` and required packages including `ssh-import-id` along with `ssh-askpass` (for use with `ssh-add)` and `gufw` (graphical interface for *UFW*) from suggested list. Then install the public key:

```bash
ssh-import-id gh:definedrisk
```

Alternatively from a remote Powershell session:

```ps
type $env:USERPROFILE\.ssh\public_key_file.pub | ssh user@server "cat >> .ssh/authorized_keys"
```

The file permissions on the server should be:

* `.ssh` directory: `700 (drwx------)`
* public key and authorized keys file (e.g. `id_rsa.pub`): `644 (-rw-r--r--)`
* private key (e.g. `id_rsa`): `600 (-rw-------)`
* lastly your home directory should not be writeable by the group or others (at most `755 (drwxr-xr-x)`).

On Ubuntu-Server the `openssh-server` package should have been selected during system installation and public key downloaded at that time. However it can be done as above from the shell.

It should now be possible to connect from a remote system. Ensure that port-forwarding is setup in VirtualBox and throughout the external network as required e.g. 2222 -> 22. Note the various options available for [virtual networking](https://www.virtualbox.org/manual/ch06.html) with Oracle VM VirtualBox.

Follow [SSH/OpenSSH/Configuring](https://help.ubuntu.com/community/SSH/OpenSSH/Configuring) noting that `sshd_config` is the configuration file for the OpenSSH server and `ssh_config` is the configuration file for the OpenSSH client. Make readonly backups before editing:

```bash
sudo cp /etc/ssh/sshd_config /etc/ssh/sshd_config.factory-defaults
sudo chmod a-w /etc/ssh/sshd_config.factory-defaults
```

Recommended change ONLY when direct access to the terminal is available **do not** do this when accessing via a remote server:

```text
#PasswordAuthentication yes
PasswordAuthentication no
```

### Install *samba* (only if required)

Help documentation:
* https://ubuntu.com/tutorials/install-and-configure-samba#1-overview
* https://help.ubuntu.com/community/Samba/SambaServerGuide
* https://help.ubuntu.com/community/How%20to%20Create%20a%20Network%20Share%20Via%20Samba%20Via%20CLI%20%28Command-line%20interface/Linux%20Terminal%29%20-%20Uncomplicated,%20Simple%20and%20Brief%20Way!
* https://www.samba.org/samba/docs/current/man-html/smb.conf.5.html

```bash
sudo apt update
sudo apt install samba -y
sudo systemctl status smbd

sudo systemctl enable --now smbd
```

Then make directories as needed:

```bash
mkdir /home/<username>/<sambashare>/
```

Remember that samba uses its own passwords which must be created for each user that will be accessing (via samba). This can include users without access to a login shell:

```bash
sudo smbpasswd -a <username>
```

Use `pdbedit` to manage SAM database (database of Samba Users):

```bash
sudo pdbedit -L -v
```

Shares can be created using *nautilus* and are saved in `/var/lib/samba/usershares/`. Alternatively edit the `smb.conf` file:

```text
[global]
    workgroup = workgroup_name
    min protocol = SMB2
    client min protocol = SMB2

    # My changes Win10
    #unix password sync = yes
    # Default is no

    #map to guest = bad user
    map to guest = never

    #usershare allow guests = yes
    # default is no

    # Disable these
    #obey pam restrictions = yes
    #pam password change = yes

[<sambashare>]
    comment = %h server (Samba, Ubuntu)
    path = /home/<username>/<sambashare>
    valid users = <username>
    readonly = no
    browsable = yes
```

Always run `testparm` after saving to check for basic syntax errors.

Finally start the service:

```bash
sudo service smbd restart
```

### Install Fonts

Download and install `ttf variable` fonts from [Cascadia Code PL](https://github.com/microsoft/cascadia-code/releases).

Install `font-manager-common` and `nautilus-font-manager` to help with this.

### Firewall and Clam AV (if required)

Use GUFW to add SSH rule then check:

```code
sudo ufw status verbose

Status: active
Logging: on (low)
Default: reject (incoming), allow (outgoing), disabled (routed)
New profiles: skip

To                         Action      From
--                         ------      ----
22/tcp                     ALLOW IN    Anywhere                  
22/tcp (v6)                ALLOW IN    Anywhere (v6)
```

See <https://help.ubuntu.com/community/ClamAV>

```bash
clamscan --version
sudo systemctl stop clamav-freshclam
sudo freshclam
sudo systemctl start clamav-freshclam
```

Setup on-access scanning and schedule scans

```bash
sudo apt-get intsall clamtk
sudo apt-get clamtk-gnome
sudo apt-get install clamav-daemonsudo ap
```

## Coding environment setup

### Install .NET SDKs

Follow instructions for SDKs from https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#2004-

Add repository:

```bash
curl -L https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb --output ~/packages-microsoft-prod.deb
sudo dpkg -i ~/packages-microsoft-prod.deb
rm ~/packages-microsoft-prod.deb
```

Then install SDKs:

```bash
sudo apt update
sudo apt-get install -y apt-transport-https && \
sudo apt-get update && \
sudo apt-get install -y dotnet-sdk-6.0 \ 
dotnet-sdk-5.0 \
dotnet-sdk-3.1
```

Or for just the runtimes:

```bash
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y aspnetcore-runtime-6.0 \
  aspnetcore-runtime-5.0 \
  aspnetcore-runtime-3.1
```

If you replace `aspnetcore` with `dotnet` the ASP.NET Core support will be ommitted from the runtime.

### Install VS Code and GIT

Install VS Code following the website instructions (download the `.deb` file) then GIT:

```bash
sudo apt install -y git git-gui git-doc
```

Create `~/.gitconfig` with:

```text
[core]
	editor = "/usr/bin/code" --wait
[user]
	name = username
	email = username@somewhere.com
```

```bash
git config --global --list --show-origin
```

Launch VS Code and sign-in to microsoft account to sync settings. Sign-in to Git-Hub account. Search for "titlebar" in settings and change from 'native' to 'custom' if not already set from synchronisation.

### Install Powershell 7 (if required)

Use Synaptic Package Manager.

### Install MSSQL-Server-2019 Express

See documentation at [Microsoft Docs: SQL Server on Linux](https://docs.microsoft.com/en-us/sql/linux/sql-server-linux-overview?view=sql-server-linux-ver15).

```bash
sudo apt update
sudo apt install -y mssql-server
```

After the package installation finishes, run `mssql-conf setup` and follow the prompts to set the SA password and choose your edition:

```bash
sudo /opt/mssql/bin/mssql-conf setup
```

If you plan to connect remotely, you might also need to open the SQL Server TCP port (default 1433) on your firewall.

Use [Visual Studio](https://docs.microsoft.com/en-us/visualstudio/data-tools/accessing-data-in-visual-studio?view=vs-2022) or [mssql extension for VS Code](https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql) to create *TestDB* database and ensure connectivity.

Install the [SQL Server command-line tools](https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-ubuntu?view=sql-server-ver16#tools) (noting that on this Ubuntu installation `.bashrc` is called from `.profile`):

```bash
sudo apt install -y mssql-tools unixodbc-dev
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc
```
