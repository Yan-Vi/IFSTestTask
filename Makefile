.PHONY: test report all clear

CONFIG ?= Debug
VERBOSITY ?= normal
ALLURE_RESULTS := IFSTests/bin/$(CONFIG)/net10.0/allure-results
ALLURE_REPORT := allure-report

test:
	dotnet test --configuration $(CONFIG) --verbosity $(VERBOSITY)

report:
	npx -y allure@3.7.0 awesome $(ALLURE_RESULTS) --single-file
	npx -y allure@3.7.0 open $(ALLURE_REPORT)

all: test report

clear:
	rm -rf $(ALLURE_RESULTS) $(ALLURE_REPORT) allure-report