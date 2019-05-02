#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Build the solution.
# --------------------------------------------------------------------------------------------------

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

info "Starting build"
withRetry dotnet build src/EnumGenerator.sln

info "Finished build"
exit 0
