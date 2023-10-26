---
title: Logs
---

### GUI

To view logs, click on the `Console` tab. Optionally, change filter from `Information` to `Debug` for more detailed logs.

To export logs, click `Help` -> `Export diagnostics...` in the top menu bar.

Sometimes OpenTabletDriver crashes hard. This is usually caused by a bug in OpenTabletDriver. Due to the crash, it will be impossible to retrieve the logs from GUI. In this case, you can find a partial log in the following locations:

| OS | Location |
| --- | --- |
| Windows | `%appdata%\OpenTabletDriver\daemon.txt` |
| Linux | `~/.local/share/OpenTabletDriver/daemon.txt` |
| macOS | `~/Library/Application Support/OpenTabletDriver/daemon.txt` |

### Daemon

The output from daemon is the log.

On Linux when running daemon via systemd service, the log for daemon is recorded by systemd. To view the log, run `journalctl -u opentabletdriver.service`.
