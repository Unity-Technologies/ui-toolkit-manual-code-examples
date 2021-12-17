#!/usr/bin/env bash

set -e

: ${image:="ads-kotlin-service-template"}

this_path="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_path="$this_path/.."

echo -e "\n\n\n--- build.sh: Building service Docker image --------------------"
docker build --pull --rm -t $image --file $repo_path/Dockerfile $repo_path
