@echo off
setlocal EnableDelayedExpansion

set textpath=%1

if "%buildConfig%"=="" (
    set buildConfig=Release
)

dotnet publish -c %buildConfig%
bin\%buildConfig%\net7.0\publish\AtCoder.exe<%textpath%>"../out.txt"
copy Combined.csx "../main.cs" /Y
