#!/bin/bash

set -euo pipefail

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

CAKE_TASK=Default

if [ "${TRAVIS_BRANCH}" == "master" ]; then
    CAKE_TASK=Docker::Publish
    docker login -u "${DOCKER_USERNAME}" -p "${DOCKER_PASSWORD}"
fi

echo "Building task ${CAKE_TASK}"

"${SCRIPT_ROOT}/../build.sh" -t "${CAKE_TASK}"
