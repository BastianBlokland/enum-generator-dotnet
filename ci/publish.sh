#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Publish NuGet packages.
# --------------------------------------------------------------------------------------------------

API_KEY=$1
if [ -z $API_KEY ]
then
    fail "Missing nuget api-key, supply as arg1"
fi

info "Start publishing"

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

publish ()
{
    info "Publish: $1"
    if fileDoesNotExist artifacts/"$1"*.nupkg
    then
        fail "Unable to find package: 'artifacts/$1*.nupkg'"
    fi

    (cd artifacts && dotnet nuget push "$1"*.nupkg \
        --api-key $API_KEY \
        --source "https://api.nuget.org/v3/index.json")
}

publish Enum.Generator.Core
publish Enum.Generator.Cli
publish Enum.Generator.GlobalTool

info "Finished publishing"
exit 0
