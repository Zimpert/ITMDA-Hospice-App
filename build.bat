@echo off

cd build
cmake -S .. 
cmake --build . -j

..\bin\debug\itmda_webserver.exe