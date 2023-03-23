#!/bin/sh

buildConfig=$3
if [ "$buildConfig" = "" ]; then
    buildConfig=Debug
fi 

current=$(cd $(dirname $0); pwd)
contestName=$(echo "$1" | sed 's/.\+/\L\0/')
contestPath=~/atcoder-workspace/$contestName
questionPath=~/atcoder-workspace/$contestName/$2

if [ ! -d $questionPath ]; then
    atcoder-tools gen $contestName
fi

cd ..
dotnet publish -c $buildConfig
cd AtCoder

cp -v -r bin/$buildConfig/netcoreapp3.1/publish/* $questionPath
cd $questionPath
atcoder-tools test -t2 -e ./AtCoder

if [ $? -eq 0 ]; then
    echo "Submit?(y/n) "
    read submit
    if [ "$submit" = "y" ]; then
        cp -v -f $current/Combined.csx $questionPath/main.cs
        atcoder-tools submit -u -t2 -e ./AtCoder
        open -a "Google Chrome" https://atcoder.jp/contests/$contestName/submissions/me
    fi
fi

cd $current