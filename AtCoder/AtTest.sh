#!/bin/sh

contestName=$(echo "$1" | sed 's/.\+/\L\0/')
contestPath=~/atcoder-workspace/$contestName
questionPath=~/atcoder-workspace/$contestName/$2

if [ ! -d $questionPath ]; then
    atcoder-tools gen $contestName
fi

cd ..
dotnet publish -c release -r osx.11.0-x64
cd AtCoder

cp -r bin/Release/netcoreapp3.1/osx.11.0-x64/* $questionPath
cp -f Program.cs $questionPath/main.cs 
cd $questionPath
atcoder-tools test

if [ $? -eq 0 ]; then
    echo "Submit?(y/n) "
    read submit
    if [ "$submit" = "y" ]; then
        atcoder-tools submit -u
        open -a "Google Chrome" https://atcoder.jp/contests/$contestName/submissions/me
    fi
fi

cd `dirname $0`