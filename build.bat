@echo off

rd /q /s build
md build
cd build
cmake -S ..
cmake --build . -j

..\bin\debug\itmda_webserver.exe