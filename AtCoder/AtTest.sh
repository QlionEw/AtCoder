#!/bin/sh

contestPath=~/atcoder-workspace/$1
questionPath=~/atcoder-workspace/$1/$2

if [ ! -d $questionPath ]; then
    atcoder-tools gen $1
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
    fi
fi

cd `dirname $0`