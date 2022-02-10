# BuildProcess
Standard build process for libraries

## Use

This process is intended to be added to a repo as a submodule within a folder directly 
below the root level of the repo, e.g. in the folder {{RepoDir}}\Build.

A build.config file placed in the root level, based on the template config file provided,
is read to provide the various options for the build.

The build can be run by executing the powershell script build.ps1.
