#!/bin/bash
set -e
source ./ci/utils.sh

# --------------------------------------------------------------------------------------------------
# Report code-coverage to codecov.io.
# --------------------------------------------------------------------------------------------------

if [ -z "$1" ]
then
    fail "No codecov repository token provided. Provide as arg1"
fi

info "Start reporting coverage"

export CODECOV_TOKEN="$1"
bash <(curl -s https://codecov.io/bash)

info "Finished reporting coverage"
exit 0
