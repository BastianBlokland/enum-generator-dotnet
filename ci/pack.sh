#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Create NuGet packages.
# --------------------------------------------------------------------------------------------------

export BUILD_NUMBER=$1
export SOURCE_SHA=$2
export BRANCH_NAME=$3

# Determine set pre-release info based on branch name
if [ $BRANCH_NAME == master ]
then
    export PRERELEASE=""
else
    export PRERELEASE="dev"
fi

info "Start packaging"
info "buildnumber: '$BUILD_NUMBER'"
info "sourcesha: '$SOURCE_SHA'"
info "branchname: '$BRANCH_NAME'"
info "prerelease: '$PRERELEASE'"

# Verify that the 'dotnet' cli command is present
verifyCommand dotnet

package ()
{
    info "Package: $1"
    dotnet pack "src/$1/$1.csproj" \
        --output "$(pwd)/artifacts" \
        --configuration Release \
        --include-source \
        --include-symbols \
        /p:TreatWarningsAsErrors=true /warnaserror
}

package Enum.Generator.Core
package Enum.Generator.Cli
package Enum.Generator.GlobalTool

info "Finished packaging"
exit 0
