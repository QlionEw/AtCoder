@echo off
pip3 install atcoder-tools --upgrade

rem そんなに使わないので一旦固定アドレスで・・・
set input=C:\Python312\Lib\site-packages\atcodertools\common\language.py
set output=C:\Python312\Lib\site-packages\atcodertools\common\language2.py

setlocal enabledelayedexpansion

for /f "delims=" %%a in (%input%) do (
    set line=%%a
    echo !line:".*C# \\(Mono.*|.*C# 11.0 "=".*C# \\(Mono.*|.*C# 11.0 AOT"!>>%output%
)

copy %output% %input%
del %output%