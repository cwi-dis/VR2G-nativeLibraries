# VR2G-nativeLibraries

Repository with VR2Gather submodule that stores native binary DLLs.

The package itself is in the subdirectory `nl.cwi.dis.vr2gather.nativelibraries`.

Add to Unity by using Package Manager -> Add Git URL and then using URL `git+https://github.com/cwi-dis/VR2G-nativeLibraries?path=/nl.cwi.dis.vr2gather.nativelibraries`.

## Maintainance

Once a new release of lldash is available do the following steps:

- Download the lldash tarball for each platform,
- Run `scripts/ingest.sh` with the 3 platform top-level directories
	- This will delete all the old dynamic libraries inside the package, and copy the new ones in. `.meta` files will be left in place.
- Open the `Ingest` project in Unity.
	- This will create any missing `.meta` files, and delete any spurious ones.
- `git` add, commit, push.

### Old

There is an old script in `.build` that was used for collecting the DLLs to package for Windows.
