# ==============================================================================
# Makefile - EnsinAI API
# Comandos facilitados para Docker e desenvolvimento
# ==============================================================================

.PHONY: help build run stop clean logs shell test

# Variáveis
IMAGE_NAME := ensinai-api
CONTAINER_NAME := ensinai-app
VERSION := latest

# Cores para output
GREEN := \033[0;32m
YELLOW := \033[0;33m
RED := \033[0;31m
NC := \033[0m # No Color

help:
	@echo "$(GREEN)EnsinAI API - Comandos disponíveis:$(NC)"
	@echo ""
	@echo "  $(YELLOW)make build$(NC)          - Build da imagem Docker"
	@echo "  $(YELLOW)make run$(NC)            - Executa com docker-compose"
	@echo "  $(YELLOW)make stop$(NC)           - Para os containers"
	@echo "  $(YELLOW)make restart$(NC)        - Reinicia os containers"
	@echo "  $(YELLOW)make logs$(NC)           - Mostra logs da aplicação"
	@echo "  $(YELLOW)make shell$(NC)          - Acessa shell do container"
	@echo "  $(YELLOW)make clean$(NC)          - Remove containers e volumes"
	@echo "  $(YELLOW)make ps$(NC)             - Lista containers ativos"
	@echo "  $(YELLOW)make test$(NC)           - Executa testes"
	@echo "  $(YELLOW)make health$(NC)         - Verifica saúde da aplicação"
	@echo "  $(YELLOW)make migrate$(NC)        - Executa migrations do banco"
	@echo ""
	@echo "$(GREEN)LocalStack (AWS S3):$(NC)"
	@echo "  $(YELLOW)make s3-list$(NC)        - Lista buckets S3"
	@echo "  $(YELLOW)make s3-files$(NC)       - Lista arquivos no bucket"
	@echo "  $(YELLOW)make s3-shell$(NC)       - Acessa AWS CLI no LocalStack"
	@echo "  $(YELLOW)make test-s3$(NC)        - Testa funcionalidade do S3"
	@echo ""

build:
	@echo "$(GREEN)🔨 Building Docker image...$(NC)"
	docker build -t $(IMAGE_NAME):$(VERSION) .
	@echo "$(GREEN)✅ Build completed!$(NC)"

run:
	@echo "$(GREEN)🚀 Starting application...$(NC)"
	docker compose up -d
	@echo "$(GREEN)✅ Application started!$(NC)"
	@echo "$(YELLOW)Access: http://localhost:8080$(NC)"
	@echo "$(YELLOW)Swagger: http://localhost:8080/swagger$(NC)"

stop:
	@echo "$(YELLOW)⏸️  Stopping containers...$(NC)"
	docker compose stop
	@echo "$(GREEN)✅ Containers stopped!$(NC)"

restart: stop run

logs:
	docker compose logs -f app

logs-all:
	docker compose logs -f

shell:
	docker exec -it $(CONTAINER_NAME) sh

clean:
	@echo "$(RED)🧹 Cleaning up...$(NC)"
	docker compose down -v
	docker rmi $(IMAGE_NAME):$(VERSION) 2>/dev/null || true
	@echo "$(GREEN)✅ Cleanup completed!$(NC)"

ps:
	docker compose ps

health:
	@echo "$(GREEN)🏥 Checking application health...$(NC)"
	@curl -s http://localhost:8080/health | jq . || echo "$(RED)❌ Application not responding$(NC)"

## migrate: Executa migrations do banco de dados
migrate:
	@echo "$(GREEN)🗄️  Running database migrations...$(NC)"
	docker exec -it $(CONTAINER_NAME) dotnet ef database update
	@echo "$(GREEN)✅ Migrations completed!$(NC)"

## test: Executa testes
test:
	@echo "$(GREEN)🧪 Running tests...$(NC)"
	dotnet test
	@echo "$(GREEN)✅ Tests completed!$(NC)"

## dev: Inicia ambiente de desenvolvimento
dev: build run
	@echo "$(GREEN)✅ Development environment ready!$(NC)"

## prod-build: Build para produção com versão
prod-build:
	@read -p "Enter version (e.g., 1.0.0): " version; \
	docker build -t $(IMAGE_NAME):$$version -t $(IMAGE_NAME):latest .
	@echo "$(GREEN)✅ Production build completed!$(NC)"

## inspect: Inspeciona a imagem Docker
inspect:
	docker inspect $(IMAGE_NAME):$(VERSION)

## size: Mostra tamanho da imagem
size:
	@echo "$(GREEN)📦 Image size:$(NC)"
	docker images $(IMAGE_NAME) --format "table {{.Repository}}\t{{.Tag}}\t{{.Size}}"

## prune: Remove recursos Docker não utilizados
prune:
	@echo "$(YELLOW)⚠️  Removing unused Docker resources...$(NC)"
	docker system prune -f
	@echo "$(GREEN)✅ Prune completed!$(NC)"

## s3-list: Lista buckets S3 no LocalStack
s3-list:
	@echo "$(GREEN)📦 S3 Buckets:$(NC)"
	docker exec -it ensinai-localstack awslocal s3 ls

## s3-files: Lista arquivos no bucket ensinai-files
s3-files:
	@echo "$(GREEN)📁 Files in bucket ensinai-files:$(NC)"
	docker exec -it ensinai-localstack awslocal s3 ls s3://ensinai-files --recursive

## s3-shell: Acessa shell do LocalStack para comandos AWS CLI
s3-shell:
	@echo "$(GREEN)🐚 LocalStack Shell (use 'awslocal' command)$(NC)"
	docker exec -it ensinai-localstack bash

## logs-localstack: Mostra logs do LocalStack
logs-localstack:
	docker compose logs -f localstack

## test-s3: Testa funcionalidade do LocalStack S3
test-s3:
	@echo "$(GREEN)🧪 Testing LocalStack S3...$(NC)"
	@bash scripts/test-localstack.sh

.DEFAULT_GOAL := help
