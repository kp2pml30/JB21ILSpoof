#!/bin/bash
set -e
export MONO_PATH="$MONO_PATH:/nuget/Mono.Cecil/lib/net40"
echo -e "\033[32mWithout modification\033[0m"
mono build/Example
echo -e "\033[32mModifing\033[0m"
mono build/Spoofer build/Example build/Result
echo -e "\033[32mResult\033[0m"
mono build/Result > build/txt
cat build/txt

echo -e "\033[32mDiff with expected\033[0m"
diff -s -U1 expected.txt build/txt

