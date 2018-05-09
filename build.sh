#!/bin/bash
nuget install fake -outputdirectory Src/packages -ExcludeVersion
mono Src/Packages/FAKE/tools/Fake.exe build.fsx