#!/bin/bash

MIGRATION_ID=$(uuidgen)

# only use this command if needed
# dotnet ef database drop

dotnet ef migrations add $MIGRATION_ID
echo Generated migration: $MIGRATION_ID
dotnet ef database update
echo Database has been updated...