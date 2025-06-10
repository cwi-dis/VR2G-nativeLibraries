#!/bin/bash
if [ $# -ne 1 ]; then
    echo Usage: $0 version
    echo
    echo This script downloads the lldash distributions for the given version and unpacks it into a tmp subdirectory
    echo specify version without the v prefix.
    exit 1
fi
set -ex
mkdir -p tmp
cd tmp
version=$1
vversion=v${version}
linux_dist=lldash-linux-x86_64-${version}.tar.gz
darwin_dist=lldash-darwin-universal-${version}.tar.gz
windows_dist=lldash-windows-amd64-${version}.tar.gz
curl --location --output ${linux_dist} https://github.com/MotionSpell/lldash/releases/download/${vversion}/${linux_dist}
curl --location --output ${darwin_dist} https://github.com/MotionSpell/lldash/releases/download/${vversion}/${darwin_dist}
curl --location --output ${windows_dist} https://github.com/MotionSpell/lldash/releases/download/${vversion}/${windows_dist}
tar xf ${linux_dist}
tar xf ${darwin_dist}
tar xf ${windows_dist}
rm ${linux_dist}
rm ${darwin_dist}
rm ${windows_dist}