@echo off
SET contest=%1
SET question=%2

set current=%CD%
set contestPath=C:\Users\owner\atcoder-workspace\%contest%
set questionPath=C:\Users\owner\atcoder-workspace\%contest%\%question%

if not exist %contestPath%\ (
    atcoder-tools gen %contest%
)

xcopy /e bin\Debug\netcoreapp3.1 %questionPath% /Y
rem copy Program.cs %questionPath%\main.cs /Y
cd %questionPath%
atcoder-tools test

rem set /P submit="Submit?(y/n)"
rem if %submit% == y (
rem     atcoder-tools submit -u
rem ) 
rem set submit=n

cd %current%