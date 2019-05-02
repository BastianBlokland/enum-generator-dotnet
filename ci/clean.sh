#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Clean previous build output.
# --------------------------------------------------------------------------------------------------

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

info "Starting clean"

# Delete old artifacts
rm -rf ./artifacts

# Clean the solution
dotnet clean src/EnumGenerator.sln

info "Finished clean"
exit 0
