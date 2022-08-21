@echo off
setlocal EnableDelayedExpansion

set contest=%1
set question=%2
set buildConfig=%3

if "%buildConfig%"=="" (
    set buildConfig=Debug
)

for %%i in (a b c d e f g h i j k l m n o p q r s t u v w x y z) do call set contest=%%contest:%%i=%%i%%

set current=%CD%
set contestPath=%HOMEPATH%\atcoder-workspace\%contest%
set questionPath=%HOMEPATH%\atcoder-workspace\%contest%\%question%

if not exist %questionPath%\ (
    atcoder-tools gen %contest%
)

dotnet publish -c %buildConfig%
xcopy /e bin\%buildConfig%\netcoreapp3.1 %questionPath% /Y
cd %questionPath%
atcoder-tools test -t1.8

if %ERRORLEVEL% == 0 (
    set submit=n
    set /P submit="Submit?(y/n) "
    if !submit! == y (
        copy %current%\Combined.csx %questionPath%\main.cs /Y
        atcoder-tools submit -u -t1.8
        start chrome https://atcoder.jp/contests/%contest%/submissions/me
    ) 
)

cd %current%