#!/bin/bash
set -ex
if [ $# -ne 3 ]; then
	echo Usage: $0 linuxdir macdir windir
	echo
	echo These are the top-level directories where the lldash distributions have been unpacked
	echo The correct files will be copied into the right location in nl.cwi.dis.vr2gather.nativelibraries
	exit 1
fi
linux_from=$1
mac_from=$2
win_from=$3
#
# Find the Unity package
#
my_path=`realpath $0`
my_dir=`dirname $my_path`
top_dir=`dirname $my_dir`
package_dir="$top_dir/nl.cwi.dis.vr2gather.nativelibraries"
linux_to="$package_dir/Runtime/Plugins/linux-x64"
mac_to="$package_dir/Runtime/Plugins/mac"
win_to="$package_dir/Runtime/Plugins/win-x64"
#
touch $linux_to/_dummy.so  $linux_to/_dummy.smd
rm $linux_to/*.{smd,so}*
cp -P $linux_from/lib/*.{smd,so}* $linux_to/
find $linux_to -type l -print0 | xargs -0 rm
#
touch $mac_to/_dummy.dylib  $mac_to/_dummy.smd
rm $mac_to/*.{smd,dylib}
cp $mac_from/lib/*.{smd,dylib} $mac_to/
#
touch $win_to/_dummy.dll $win_to/_dummy.smd
rm $win_to/*.{smd,dll}
cp $win_from/bin/*.{smd,dll} $win_to/
#
echo Please open the Importer project to fix up the meta files, and then checkin to git.
