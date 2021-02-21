dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools/reportgenerator
dotnet test --collect:"XPlat Code Coverage"
"tools/reportgenerator/reportgenerator.exe" "-reports:RuleEngine.Test/TestResults/*/*.xml" "-targetdir:coverage" -reporttypes:Html;HtmlChart