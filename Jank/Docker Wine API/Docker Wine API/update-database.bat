@echo off

cd WineApi

dotnet tool install --global dotnet-ef

dotnet ef database update 