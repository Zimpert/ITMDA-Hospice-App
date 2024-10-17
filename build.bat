@echo off

cd build
cmake -S ..
cmake --build .

..\bin\debug\itmda_webserver.exe