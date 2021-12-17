#!/usr/bin/env bash

this_path="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
repo_path="$this_path/.."

trap "docker-compose down" SIGINT SIGTERM EXIT

echo -e "\n\n\n --- test.sh: Creating build folder to repository path $repo_path ----------------------"
rm -rf "$repo_path/build"
mkdir -p "$repo_path/build"
chmod a+rwx "$repo_path/build"

echo -e "\n\n\n --- test.sh: Building the image  -------------------------------"
docker-compose build

echo -e "\n\n\n --- test.sh: Running unit tests -------------------------------"
if docker-compose run -v $repo_path/build:/build ads-kotlin-service-template-builder gradle build -x jar -x distTar -x distZip
then
  docker-compose stop
  exit 0
else
  docker-compose stop
  exit 1
fi
