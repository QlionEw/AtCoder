@echo off
setlocal EnableDelayedExpansion

set contest=%1
set question=%2

for %%i in (a b c d e f g h i j k l m n o p q r s t u v w x y z) do call set contest=%%contest:%%i=%%i%%

set current=%CD%
set contestPath=%HOMEPATH%\atcoder-workspace\%contest%
set questionPath=%HOMEPATH%\atcoder-workspace\%contest%\%question%

if not exist %questionPath%\ (
    atcoder-tools gen %contest%
)

dotnet publish -c release
xcopy /e bin\Release\netcoreapp3.1 %questionPath% /Y
copy Program.cs %questionPath%\main.cs /Y
cd %questionPath%
atcoder-tools test -t1800

if %ERRORLEVEL% == 0 (
    set submit=n
    set /P submit="Submit?(y/n) "
    if !submit! == y (
        atcoder-tools submit -u -t1800
        start chrome https://atcoder.jp/contests/%contest%/submissions/me
    ) 
)

cd %current%