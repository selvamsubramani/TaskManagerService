﻿@echo off

rem CodeCoverage using Open Cover

%~dp0packages\OpenCover.4.6.166\tools\OpenCover.Console.exe -target:"%PROGRAMFILES(X86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\MSTest.exe" -targetargs:"/noisolation /testcontainer:TaskManager.Test\bin\Release\TaskManager.Test.dll" -register:user -output:TestResult.xml -excludebyattribute:*.ExcludeFromCoverage* -mergebyhash -skipautoprops

exit