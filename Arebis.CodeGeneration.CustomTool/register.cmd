@echo off
REM regasm must be in your path
REM regasm can be found in your Microsoft.NET SDK
regasm /codebase bind\release\Arebis.CodeGeneration.CustomTool.dll
pause