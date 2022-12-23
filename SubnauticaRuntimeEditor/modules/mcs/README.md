# Mono.CSharp 

This is an edit of the official Mono.CSharp codebase with edits for Unity modding:

* Backport C# 7 compiler code to .NET 3.5 (taken from https://github.com/kkdevs/mcs)
* Remove dependency on System.XML as some games don't ship with it
* Force Evaluator to import all memebers for code completion
* Ignore access checks during compilation (by overwriting `MemberSpec.IsAccessible`)
* Force the JIT to ignore access checks by marking the generated assembly as `corlib_internal`
