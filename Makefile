PACKAGE			:= MetricServer.AspNetCore
OWNER			:= dfds
REPO			:= MetricServer
PROJECT			:= github.com/$(OWNER)/$(REPO)
CONFIGURATION	:= Debug
VERSION			?= $(shell git describe --tags --always --dirty --match=*.*.* 2> /dev/null || cat $(CURDIR)/.version 2> /dev/null || echo 0.0.1)
BIN				:= $(CURDIR)/.output
M				= $(shell printf "\033[34;1m▶\033[0m")

PROJECT_FILE=${PWD}/src/Dafda/
OUTPUT_DIR=${PWD}/.output

.PHONY: all
all: build

.PHONY: init
init: restore build ## restore, build

.PHONY: clean
clean: ; $(info $(M) Cleaning...) @ ## clean the build artifacts
	@rm -rf $(BIN)

.PHONY: restore
restore: ; $(info $(M) Restoring dependencies...) @ ## restore project dependencies
	@cd src && dotnet restore

.PHONY: build
build: ; $(info $(M) Building...) @ ## build the project
	@cd src && dotnet build --configuration $(CONFIGURATION)

.PHONY: package
package: ; $(info $(M) Packing...) @ ## create nuget package
	@cd src && dotnet pack --no-build --configuration $(CONFIGURATION) -p:PackageVersion=$(VERSION) --output $(BIN) $(CURDIR)/src/$(PACKAGE)/
	
.PHONY: local-release
local-release: clean restore build package ## create a nuget package for local development

.PHONY: release
release: CONFIGURATION=Release ## create a release nuget package
release: clean restore build package

.PHONY: push
push: require ## push nuget package
	cd $(OUTPUT_DIR) && dotnet nuget push *.nupkg --source https://api.nuget.org/v3/index.json --api-key $(NUGET_API_KEY)

.PHONY: require
require:
ifndef NUGET_API_KEY
	$(error NUGET_API_KEY is undefined)
endif

.PHONY: version
version: ## prints the version (from either environment VERSION, git describe, or .version. default: v0.0.1)
	@echo $(VERSION)

.PHONY: help
help:
	@grep -E '^[ a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | \
		awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-15s\033[0m %s\n", $$1, $$2}'
