@echo off
setlocal EnableDelayedExpansion

set textpath=%1

if "%buildConfig%"=="" (
    set buildConfig=Release
)

dotnet publish -c %buildConfig%
AtCoder\bin\%buildConfig%\net7.0\publish\AtCoder.exe<%textpath%>"out.txt"
copy AtCoder\Combined.csx "main.cs" /Y
