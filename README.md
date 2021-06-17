# max-tools

A repository of C# WPF tool(s) for 3ds Max.

# Building

To create a release build, open powershell in the root of this repository and run (the `--clean` is optional).
```
.\devops\build.bat --release --clean
```

You can also create debug releases with:
```
.\devops\build.bat --debug --clean
```

Builds are emitted to the **out/** folder and are also zipped for the convenience of distribution.

# Running

Once the build has succeeded, navigate to the relevant folder, for example **out/Debug_2020** and run the `bulk-edit.ms` MAXScript file in 3ds Max. It should open a window which updates based on your current selection, allowing you to edit user-defined properties.
