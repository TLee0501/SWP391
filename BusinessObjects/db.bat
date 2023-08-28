@echo off
SETLOCAL

REM Generate a UUID
FOR /F %%i IN ('powershell -Command "[guid]::NewGuid().ToString()"') DO SET 
MIGRATION_ID=%%i

dotnet ef migrations add %MIGRATION_ID%
echo Generated migration: %MIGRATION_ID%
dotnet ef database update
echo Database has been updated...

ENDLOCAL
