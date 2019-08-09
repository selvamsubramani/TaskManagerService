@echo off

rem CodeCoverageReport using ReportGenerator

"%~dp0packages\ReportGenerator.3.1.2\tools\ReportGenerator.exe" -reports:"TaskManager.Test\bin\Release\TestResult.xml" -targetdir:"CodeCoverage-OpenCover"

exit
