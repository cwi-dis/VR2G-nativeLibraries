#!/bin/bash
set -x
depends="/c/Users/DIS/Dependencies_x64_Release/Dependencies.exe"
echo > tmp.depends.txt
for lib in *.dll; do
	$depends -modules $lib | grep Environment >> tmp.depends.txt
done
sort -u tmp.depends.txt > tmp.depends-unique.txt
# Lines are now of the form
# [Environment] OpenNI2.dll : C:\Program Files\OpenNI2\Redist\OpenNI2.dll
sed -e 's/.Environment. \(.*\) : \(.*\) /x="`cygpath -u "\2"`" ; cp "$x" ./' <tmp.depends-unique.txt > tmp.copy.sh
sh -x tmp.copy.sh