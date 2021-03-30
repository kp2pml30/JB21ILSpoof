#!/bin/bash
set -e
export MONO_PATH="$MONO_PATH:/nuget/Mono.Cecil"
RS=""
for i in ${MONO_PATH//:/ }
do
	for dll in $(find "$i" -name '*Cecil.dll' ! -path '*netstan*')
	do
		RS="$RS -r:$dll"
	done
done
mkdir -p build
csc Example.cs -out:build/Example
csc Program.cs Spoofer.cs -out:build/Spoofer "$RS"
