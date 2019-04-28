#!/bin/bash
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Run the xUnit tests and collect coverage using CoverLet.
# --------------------------------------------------------------------------------------------------

TEST_RESULT_PATH="../../artifacts/xunit.results.xml"
COVERAGE_RESULT_PATH="../../artifacts/coverage.cobertura.xml"
REPORT_RESULT_PATH="../../artifacts/coverage_report"

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

info "Starting tests"

# Build the solution in Debug configuration (So that Debug.Asserts will fire)
withRetry dotnet build --configuration Debug src/Enum.Generator.sln

# Run test (And collect coverage using coverlet)
dotnet test src/Enum.Generator.Tests/Enum.Generator.Tests.csproj \
    --logger "xunit;LogFilePath=$TEST_RESULT_PATH" \
    /p:CollectCoverage=true \
    /p:UseSourceLink=true \
    /p:Include="[Enum.Generator.Core]*" \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=$COVERAGE_RESULT_PATH
EXIT_CODE=$?

info "Finished tests"
exit $EXIT_CODE
