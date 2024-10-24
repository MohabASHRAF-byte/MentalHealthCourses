#!/bin/bash

# Check if a migration name was provided
if [ -z "$1" ]; then
    echo "Error: No migration name provided."
    echo "Usage: ./update_database.sh <MigrationName>"
    exit 1
fi

MIGRATION_NAME=$1

# Run the commands sequentially with '&&'
dotnet ef migrations add "$MIGRATION_NAME" --project MentalHealthcare.Infrastructure/ --startup-project MentalHealthcare.API/ &&
cd MentalHealthcare.API/ &&
dotnet ef database update &&
cd .. &&
echo "Database update complete."
