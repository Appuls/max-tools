@echo off
setlocal enabledelayedexpansion

REM Usage:
REM For all Debug builds:
REM    > build_all.bat --debug
REM
REM For all Release builds:
REM    > build_all.bat --release

set HERE=%~dp0
set REPO_ROOT=!HERE!..\
set SLN=!REPO_ROOT!max-tools.sln
set SRC=!REPO_ROOT!src\
set OUT=!REPO_ROOT!out\

set DEBUG_OR_RELEASE=Debug
set CLEAN=
REM set MSBUILDEMITSOLUTION=1

for %%a in (%*) do (
  if "%%a"=="--debug" (
    set DEBUG_OR_RELEASE=Debug
  )
  if "%%a"=="--release" (
    set DEBUG_OR_RELEASE=Release
  )
  if "%%a"=="--clean" (
    set CLEAN="true"
  )
)

if !CLEAN!=="true" (
  echo Cleaning "!SRC!" ...
  pushd "!SRC!"
  call git clean -fxd .
  popd
  echo.

  echo Cleaning "!OUT!" ...
  pushd "!OUT!"
  call git clean -fxd .
  popd
  echo.
)

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
