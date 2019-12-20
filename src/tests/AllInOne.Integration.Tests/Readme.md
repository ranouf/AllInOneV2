# AllInOne.Integration.Tests

To Generate an Html Code Coverage report, execute:

- cd src/tests/AllInOne.Integration.Tests/
- dotnet test   /p:CollectCoverage=true   /p:CoverletOutputFormat=cobertura /p:CoverletOutput="./reports/"
- dotnet reportgenerator "-reports:.\reports\coverage.cobertura.xml" "-targetdir:./reports"

