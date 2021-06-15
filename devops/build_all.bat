@echo off
setlocal enabledelayedexpansion

REM Usage:
REM For all Debug builds:
REM    > build_all.bat Debug
REM
REM For all Release builds:
REM    > build_all.bat Release

set HERE=%~dp0
set SLN="!HERE!..\max-tools.sln"
set DEBUG_OR_RELEASE=%1
REM set MSBUILDEMITSOLUTION=1

for %%a in (
  "2020"
  "2021"
  "2022"
) do (
  set YEAR=%%~a
  set CFG="!DEBUG_OR_RELEASE!_!YEAR!"
  echo Building Configuration !CFG!
  echo.
  call msbuild "!SLN!" -target:Restore;MaxToolsLib:Rebuild -maxcpucount -property:Configuration=!CFG! -verbosity:quiet
)
