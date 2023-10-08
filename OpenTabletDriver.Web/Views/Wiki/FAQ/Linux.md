---
title: "Linux FAQ"
---

### My tablet is not detected? {#tablet-not-detected}

For sanity reasons, make sure that your tablet is actually plugged in to your computer, and is detected by your OS.

You can check this with `lsusb` or a GUI utility. `dmesg` may also be handy to inspect USB plug or unplug events

Once you are 100% sure your tablet is connected to your computer, and your kernel is seeing it,
ensure that your specific tablet make and model is supported by checking our list of supported tablets [here](/Tablets).

If you plugged in your tablet _before_ installing OpenTabletDriver, please replug your tablet, otherwise detection might be impaired.

We currently do not support tablets that connect directly to a computer via Bluetooth,
and there is only limited support for tablets that use wireless USB receivers, so please make sure your tablet is connected via cable.

If you installed OpenTabletDriver with a package manager, and replugging the tablet does not help, you might need to reboot your computer.

Please reboot before attempting the solutions below to avoid misconfiguration.

#### It is a supported tablet, but still doesn't detect after a reboot?

Look for a log that resembles the following:

<small class="text-muted">Hint: You can see the console log under the "Console" tab in OpenTabletDriver.UX</small>

##### hid_uclogic kernel module interference {#hid_uclogic}

> Another tablet driver found: UC Logic\
> ArgumentOutOfRangeException Value range is [0, 15]. (Parameter 'value')

These errors may happen if the `hid_uclogic` kernel module is loaded at the same time as OpenTabletDriver.

You can remove and blacklist this module to stop it from loading in the future by running the following commands:
```bash
echo "blacklist hid_uclogic" | sudo tee -a /etc/modprobe.d/blacklist.conf
sudo rmmod hid_uclogic
```

##### Missing udev rules {#udev}

> Not permitted to open HID class device at /dev/hidrawX

If you have followed the previous instructions, usually this is the only reason OpenTabletDriver will fail to detect your tablet when it's supported.

The issue is resolved by making sure that you have proper udev rules set up.

###### From Package Manager

If you installed OpenTabletDriver via your package manager, you may just need to reload and trigger the rules:
```bash
sudo udevadm control --reload-rules
sudo udevadm trigger
```

If it still doesn't work after this point, and you installed OpenTabletDriver with a package manager, please reboot, some systems may require this.

###### Portable install

If you installed OpenTabletDriver without using a package manager you will not have proper udev rules set up and must build your own.
It is highly recommended that you use your package manager where available.

This process will require you to have the dotnet 7 SDK installed before proceeding:
```bash
# Clone the repository, change current directory to the repository
git clone https://github.com/OpenTabletDriver/OpenTabletDriver.git
cd ./OpenTabletDriver

# Generate rules, moves them to the udev rules directory
./generate-rules.sh
sudo mv ./bin/99-opentabletdriver.rules /etc/udev/rules.d/99-opentabletdriver.rules

# Reload udev rules
sudo udevadm control --reload-rules

# Clean up leftovers
cd ..
rm -rf OpenTabletDriver
```
Then restart OpenTabletDriver and replug your tablet.

#### It is not a supported tablet, what can I do about that?

If you want to add support for your tablet on your own, feel free to look at a similar configuration on our GitHub and our configuration documentation.

If you would like help with supporting your tablet, or would prefer us to do it, join our [Discord](/Discord) and create a post in `#config-creation` or a support channel.

### My cursor is teleporting? {#ghost-cursor}

This occurs when another program is reading the tablet at the same time as OpenTabletDriver, if this is happening everywhere on the system,
the solution to this is very simple:
```bash
echo "blacklist wacom" | sudo tee -a /etc/modprobe.d/blacklist.conf
sudo rmmod wacom
```
If you are using a Wayland compositor, it may be required for you use artist mode to pass the tablet as a virtual tablet
rather than an absolute mouse because certain applications like SDL and XWayland may incorrectly handle the cursor otherwise.
This is a rather simple setup which you can find <a href="#artist-mode">here</a>.

### I blacklisted kernel modules, but they load on startup? {#blacklisted-modules-loading}

This issue can be caused by either your initramfs not containing the instructions to not load the modules
or another crucial service loading the module despite it being blacklisted. You will have to find that service
yourself if the solution below does not solve your issue.

Rebuilding the initramfs is an easy process, but the commands to do this vary between the distro.

#### Arch

```bash
sudo mkinitcpio -P
```

#### Debian / Ubuntu
```bash
sudo update-initramfs -u
```

### My tablet detects, but it isn't working? {#fail-virtual-device}

If this is on a fresh install of OpenTabletDriver, usually problems like this can be solved
simply by restarting your computer. If it isn't resolved you may try resetting your OpenTabletDriver
settings to the defaults with `File > Reset to defaults`, then pressing save.

If your issue is still not resolved, and you are using OpenTabletDriver.UX, navigate to the console tab
at the top of the main interface, if you are using the daemon directly you may simply just look for a log
that resembles the following:

#### EACCESS

> Failed to initialize virtual tablet. (error code EACCES)

This usually occurs when udev is improperly set up or hasn't explictly been reloaded.

Make sure your rules are correct by heading [here](#udev).

#### ENODEV

> Failed to initialize virtual tablet. (error code ENODEV)

This error occurs after updating the kernel but without rebooting, you should reboot your computer
and check if the problem is resolved.

#### ENOENT
This error occurs when the `CONFIG_INPUT_UINPUT` kernel option is disabled. Make sure it is enabled in
your kernel config.

##### Gentoo
Check the option `CONFIG_INPUT_UINPUT` in /usr/src/linux.config. If it is not enabled, use the following steps

```bash
cd /usr/src/linux
sudo make menuconfig
```

Having done that, select y (for always active) or m (for building as a module) for the option in
*Device Drivers ---> Input Device Support ---> Miscellaneous devices ---> User level driver support* and rebuild the kernel.

##### Other Distros

Refer to your distribution's documentation regarding kernel configuration to enable this option.

This is probably due to your kernel not having uinput built in, if `stat /dev/uinput` doesn't return a file and you are using a custom kernel,
compile your kernel with support for uinput or use a different kernel.

### Poor performance with NVIDIA {#performance-nvidia}

Disable "Force full composition pipeline" in the NVIDIA settings panel.

### Stuck cursor in osu!lazer (Wayland) {#osu-lazer-broken-input-wayland}

Make sure you set the `SDL_VIDEODRIVER` to `wayland`:

```bash
env SDL_VIDEODRIVER=wayland ./osu.AppImage
```
or
```bash
env SDL_VIDEODRIVER=wayland osu-lazer
```

### My CTL-x100 is not detected? {#CTL-x100-android-mode}
It is possible for CTL-x100 tablets boots in Android mode instead of PC mode in some rare cases. To fix this, press
the outer 2 express keys for 3-4 seconds until the LEDs change brightness. This toggles the tablet's operating mode
between PC (high LED brightness) and Android mode (low LED brightness).

<small class="text-muted">
Note: If you are sure your tablet is in PC mode, please follow our generalised instructions [here](#tablet-not-detected)
</small>

### The systemd user service doesn't automatically start? {#systemd-autostart}

If you have enabled the systemd user service and this happens,
your distro isn't properly activating `graphical-session.target` to ensure OpenTabletDriver starts cleanly.

If you are experiencing this, add `otd-daemon` to the autostart of your window manager for OpenTabletDriver to work on startup.

### How do I start the driver when I'm not using systemd? {#non-systemd}

On non-systemd distros you do have not have a specified way to automatically start the daemon.
Instead, if you have installed a packaged build you can run `otd-daemon` in a terminal

If you are not using a packaged build and instead compiled from source,
you will need to run `./OpenTabletDriver.Daemon` in your terminal from the build directory instead.

You may place this in your window managers startup for it to automatically start.

### Enabling pressure using Artist Mode {#artist-mode}

Pressure support is available by changing the output mode of OpenTabletDriver to Artist Mode:

- Change output mode (at the bottom left of OpenTabletDriver) to Artist Mode.
- Remove the Tip Binding in the Pen Settings panel by clicking the 3 dots, then pressing clear.
- Save your settings and try drawing in an applicantion that supports pressure.

You will also need to use artist mode mouse bindings in the advanced binding editor (press '...' next to the binding)
instead of regular mouse buttons. Regular mouse buttons are not currently supported in artist mode.

### Disabling libinput's tablet device smooething

Using artist mode will result in some minor smoothing due to libinput's tablet handling.

To disable this smoothing add the contents below to `/etc/libinput/local-overrides.quirks`:
```ini
[OpenTabletDriver Virtual Tablets]
MatchName=OpenTabletDriver*
AttrTabletSmoothing=0
```

You should restart the OpenTabletDriver daemon after updating this file.

### I have tried these solutions, but my problem still persists! {#discord}

If you are still encountering problems with OpenTabletDriver,
it will be easier to help you over in our [Discord](/Discord) server,
where we may ask you to do certain debugging steps and give you different instructions to help resolve your problem.
