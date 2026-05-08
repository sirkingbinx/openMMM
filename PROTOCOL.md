## `mmm://` URI Protocol
To allow for basically anything to integrate with MonkeModManager, we use the `mmm` protocol. Basically any major programming language supports it, web browsers can use it gracefully, and it's easy to use.

This doc will go over all of the features that the MMM protocol supports.

## `mmm://install`
`install` allows you to install mod loaders and mods.

To pass a mod, you add it like this:
```txt
Utilla~https://github.com/sirkingbinx/Utilla/releases/latest/download/Utilla.dll
```
Using whatever programming language you choose, make the text URL safe (excluding the ~):
```
Utilla~https%3A%2F%2Fgithub.com%2Fsirkingbinx%2FUtilla%2Freleases%2Flatest%2Fdownload%2FUtilla.dll
```
To install dependencies, list them in reverse order of installation, seperated by dashes (-).
```
Main%20Mod~https%3A%2F%2Fexample.com%2Fmod3.dll-Dependency~https%3A%2F%2Fexample.com%2Fmod2.dll-Dependency%20of%20Dependency~https%3A%2F%2Fexample.com%2Fmod3.dll
```
If your download is a zip file, it will be extracted to the directly holding Gorilla Tag.exe (the GT root directory)
```txt
My%20Zipped%20Mod~https%3A%2F%2Fexample.com%2Fmod.zip
```

Add your mods to the `install` request like this:
```
mmm://install?mods=Main%20Mod~https%3A%2F%2Fexample.com%2Fmod3.dll-Dependency~https%3A%2F%2Fexample.com%2Fmod2.dll
```

Optionally, install a mod loader. BepInEx will be installed if no loader is present when a mod is installed.
```
mmm://install?loader=[bepinex / melonloader]
```