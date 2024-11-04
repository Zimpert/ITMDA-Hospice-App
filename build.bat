@echo off

REM rd /q /s build
REM md build
cd build
cmake -S ..
cmake --build . -j

..\bin\debug\itmda_webserver.exe