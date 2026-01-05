# Docker Swarm Migration Guide
## From Docker Compose to Production-Ready Orchestration

---

## Table of Contents
1. [Docker Compose vs Docker Swarm](#1-docker-compose-vs-docker-swarm)
2. [Architecture Overview](#2-architecture-overview)
3. [Docker Stack Configuration](#3-docker-stack-configuration)
4. [Frontend Dockerization](#4-frontend-dockerization)
5. [Docker Swarm Commands](#5-docker-swarm-commands)
6. [Service Discovery & Routing](#6-service-discovery--routing)
7. [Production Best Practices](#7-production-best-practices)
8. [Troubleshooting](#8-troubleshooting)

---

## 1. Docker Compose vs Docker Swarm

### Docker Compose
**Purpose**: Development and testing environment orchestration

**Key Characteristics**:
- ✅ **Single-host deployment** - Runs all containers on one machine
- ✅ **Simple configuration** - YAML file for service definition
- ✅ **Quick setup** - Ideal for local development
- ❌ **No built-in scaling** - Manual container replication
- ❌ **No load balancing** - No distributed request handling
- ❌ **No high availability** - Single point of failure
- ❌ **Limited health monitoring** - Basic healthchecks only

**Best for**: 
- Local development
- Testing microservices interactions
- CI/CD pipelines
- Small-scale deployments

---

### Docker Swarm
**Purpose**: Production-grade container orchestration

**Key Characteristics**:
- ✅ **Multi-host deployment** - Distributed across multiple nodes
- ✅ **Native clustering** - Built into Docker Engine
- ✅ **Automatic load balancing** - Ingress routing mesh
- ✅ **Service scaling** - `docker service scale` command
- ✅ **Self-healing** - Automatic restart of failed containers
- ✅ **Rolling updates** - Zero-downtime deployments
- ✅ **Secret management** - Encrypted credential storage
- ✅ **Overlay networking** - Secure multi-host communication

**Best for**:
- Production environments
- High-availability applications
- Scalable microservices
- Teams transitioning from Docker Compose

---

### Comparison Table

| Feature | Docker Compose | Docker Swarm |
|---------|---------------|--------------|
| **Deployment** | Single machine | Multi-machine cluster |
| **Scaling** | Manual (`docker-compose scale`) | Automatic (`docker service scale`) |
| **Load Balancing** | None | Built-in ingress routing |
| **High Availability** | No | Yes (replicas across nodes) |
| **Updates** | Recreate containers | Rolling updates |
| **Networking** | Bridge network | Overlay network |
| **Secrets** | Environment variables | Encrypted Docker secrets |
| **Health Checks** | Basic | Advanced with policies |
| **Learning Curve** | Low | Medium |

---

## 2. Architecture Overview

### Current Docker Compose Architecture
```
┌─────────────────────────────────────────────────┐
│              Bridge Network                      │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐      │
│  │ Eureka   │  │ Config   │  │ Gateway  │      │
│  │ :8761    │  │ :9999    │  │ :8080    │      │
│  └──────────┘  └──────────┘  └──────────┘      │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐      │
│  │Identity  │  │ Catalog  │  │  Cart    │      │
│  │ :5001    │  │ :5002    │  │ :5003    │      │
│  └──────────┘  └──────────┘  └──────────┘      │
│  ┌──────────┐                                   │
│  │  Order   │                                   │
│  │ :5004    │                                   │
│  └──────────┘                                   │
└─────────────────────────────────────────────────┘
        ↓ npm start (external)
    Frontend :3000
```

### Docker Swarm Architecture
```
┌──────────────────────────────────────────────────────────┐
│                  Overlay Network                          │
│                 (microservices-overlay)                   │
│                                                           │
│  ┌────────────────────────────────────────────┐          │
│  │          Manager Node(s)                    │          │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────┐ │          │
│  │  │ Eureka   │  │ Config   │  │Visualizer│ │          │
│  │  │ (1 rep)  │  │ (1 rep)  │  │ :9090    │ │          │
│  │  └──────────┘  └──────────┘  └──────────┘ │          │
│  └────────────────────────────────────────────┘          │
│                                                           │
│  ┌────────────────────────────────────────────┐          │
│  │          Worker Node(s)                     │          │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────┐ │          │
│  │  │ Gateway  │  │ Gateway  │  │ Catalog  │ │          │
│  │  │(replica1)│  │(replica2)│  │(replica1)│ │          │
│  │  └──────────┘  └──────────┘  └──────────┘ │          │
│  │  ┌──────────┐  ┌──────────┐  ┌──────────┐ │          │
│  │  │ Identity │  │ Identity │  │  Cart    │ │          │
│  │  │(replica1)│  │(replica2)│  │(replica1)│ │          │
│  │  └──────────┘  └──────────┘  └──────────┘ │          │
│  │  ... (more replicas distributed)           │          │
│  └────────────────────────────────────────────┘          │
│                                                           │
│  ┌────────────────────────────────────────────┐          │
│  │          Ingress Routing Mesh               │          │
│  │      (Load balances :8080 requests)         │          │
│  └────────────────────────────────────────────┘          │
└──────────────────────────────────────────────────────────┘
              ↑
        External Access
    http://localhost:8080 (Gateway)
    http://localhost:9090 (Visualizer)
```

**Key Improvements**:
1. **Overlay Network**: Secure encrypted communication between services across hosts
2. **Service Replicas**: Multiple instances for load distribution and fault tolerance
3. **Internal-Only Services**: Backend services not exposed directly (security)
4. **Ingress Routing**: Automatic load balancing to gateway replicas
5. **Visualization**: Real-time cluster monitoring

---

## 3. Docker Stack Configuration

### Key Changes from docker-compose.yml to docker-stack.yml

#### 1. **Network Type**
```yaml
# Docker Compose (Bridge)
networks:
  devops-net:
    driver: bridge

# Docker Swarm (Overlay)
networks:
  microservices-overlay:
    driver: overlay
    attachable: true
```

**Why Overlay?**
- Supports multi-host communication
- Built-in service discovery via DNS
- Encrypted by default
- Allows seamless scaling across nodes

---

#### 2. **Service Replicas**
```yaml
# Docker Compose (Single instance)
catalog-service:
  build: ./backend/catalog-service
  container_name: catalog-service
  ports:
    - "5002:8080"

# Docker Swarm (Multiple replicas)
catalog-service:
  image: tp-catalog-service
  build: ./backend/catalog-service
  # No container_name in Swarm
  # No external ports (internal only)
  deploy:
    replicas: 3  # 3 instances for high availability
    update_config:
      parallelism: 1
      delay: 10s
    restart_policy:
      condition: on-failure
```

**Why Replicas?**
- **Load Distribution**: Requests spread across instances
- **Fault Tolerance**: Service continues if one instance fails
- **Zero-Downtime Updates**: Rolling updates with parallelism control

---

#### 3. **Port Exposure Strategy**
```yaml
# Only Gateway is exposed externally
gateway-service:
  ports:
    - target: 8080
      published: 8080
      protocol: tcp
      mode: host

# Backend services (NO external ports)
catalog-service:
  # Accessible via service name only
  # Example: http://catalog-service:8080/products
```

**Security Benefits**:
- Reduced attack surface (only gateway exposed)
- Backend services isolated in overlay network
- API Gateway acts as single entry point

---

#### 4. **Visualizer Service**
```yaml
visualizer:
  image: dockersamples/visualizer:latest
  ports:
    - "9090:8080"
  volumes:
    - /var/run/docker.sock:/var/run/docker.sock
  deploy:
    replicas: 1
    placement:
      constraints:
        - node.role == manager
```

**Features**:
- Real-time visualization of services and nodes
- Shows which containers run on which nodes
- Displays service health and replica count
- Accessible at: `http://localhost:9090`

---

## 4. Frontend Dockerization

### Multi-Stage Dockerfile Explanation

```dockerfile
# ============ Stage 1: Build ============
FROM node:18-alpine AS build

WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production  # Clean install
COPY . .
RUN npm run build  # Creates optimized production build

# ============ Stage 2: Serve ============
FROM nginx:alpine

COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

**Why Multi-Stage?**
1. **Stage 1 (Build)**: Uses Node.js to compile React app
   - Installs dependencies
   - Runs webpack/build process
   - Creates optimized static files

2. **Stage 2 (Serve)**: Uses lightweight Nginx
   - Copies only built files (not source code)
   - Final image ~23MB (vs ~1GB with Node.js)
   - Production-ready web server

**Benefits**:
- **Small Image Size**: Only production artifacts included
- **Fast Deployment**: Smaller images = faster pulls
- **Security**: No build tools in production image
- **Performance**: Nginx serves static files efficiently

---

### Adding Frontend to Stack

Uncomment in `docker-stack.yml`:
```yaml
frontend:
  image: tp-frontend
  build: ./frontEnd/ecommerce-sesame
  ports:
    - "3000:80"
  networks:
    - microservices-overlay
  deploy:
    replicas: 2
    update_config:
      parallelism: 1
      delay: 10s
  depends_on:
    - gateway-service
```

---

## 5. Docker Swarm Commands

### Initial Setup

#### 1. Initialize Swarm (Run on Manager Node)
```bash
docker swarm init --advertise-addr <MANAGER-IP>

# Example output:
# Swarm initialized: current node (abc123) is now a manager.
# To add a worker to this swarm, run:
#   docker swarm join --token SWMTKN-1-xxx <MANAGER-IP>:2377
```

**What it does**:
- Creates a Swarm cluster
- Current machine becomes manager node
- Generates join tokens for workers

---

#### 2. Add Worker Nodes (Optional)
```bash
# On worker machine:
docker swarm join --token SWMTKN-1-xxx <MANAGER-IP>:2377
```

---

### Stack Deployment

#### 3. Build Images (First Time Only)
```bash
# Build all service images
docker-compose -f docker-stack.yml build

# Or build individually:
docker build -t tp-gateway-service ./gateway-service
docker build -t tp-catalog-service ./backend/catalog-service
# ... etc
```

---

#### 4. Deploy Stack
```bash
docker stack deploy -c docker-stack.yml microservices

# Expected output:
# Creating network microservices_microservices-overlay
# Creating service microservices_eureka-server
# Creating service microservices_config-server
# Creating service microservices_gateway-service
# Creating service microservices_identity-service
# Creating service microservices_catalog-service
# Creating service microservices_cart-service
# Creating service microservices_order-service
# Creating service microservices_visualizer
```

**Parameters**:
- `-c`: Compose file path
- `microservices`: Stack name (choose any name)

---

### Monitoring & Management

#### 5. List Services
```bash
docker service ls

# Example output:
# ID          NAME                              MODE      REPLICAS  IMAGE
# abc123      microservices_gateway-service     replicated  2/2     tp-gateway-service
# def456      microservices_catalog-service     replicated  3/3     tp-catalog-service
# ghi789      microservices_visualizer          replicated  1/1     dockersamples/visualizer
```

**Columns Explained**:
- **MODE**: `replicated` (multiple instances) or `global` (one per node)
- **REPLICAS**: `running/desired` - shows actual vs expected count
- **IMAGE**: Docker image used

---

#### 6. Inspect Service Details
```bash
# View service configuration
docker service inspect microservices_gateway-service --pretty

# View service logs
docker service logs microservices_gateway-service -f

# View tasks (individual container instances)
docker service ps microservices_gateway-service
```

---

#### 7. Scale Services
```bash
# Scale catalog service to 5 replicas
docker service scale microservices_catalog-service=5

# Scale multiple services at once
docker service scale \
  microservices_identity-service=3 \
  microservices_cart-service=4

# Verify scaling
docker service ls
```

**Use Cases**:
- **High Traffic**: Increase gateway/catalog replicas
- **Low Usage**: Reduce replicas to save resources
- **Maintenance**: Scale to 0 for temporary shutdown

---

#### 8. Update Services (Rolling Updates)
```bash
# Update service image
docker service update --image tp-catalog-service:v2 microservices_catalog-service

# Update environment variable
docker service update --env-add NEW_VAR=value microservices_gateway-service

# Rollback to previous version
docker service rollback microservices_catalog-service
```

**Rolling Update Process**:
1. Stops 1 container (parallelism: 1)
2. Starts new version
3. Waits for healthcheck
4. Repeats for next container
5. Zero downtime achieved ✅

---

#### 9. Remove Stack
```bash
# Remove entire stack
docker stack rm microservices

# This removes:
# - All services
# - Networks (if not used elsewhere)
# - Does NOT remove images or volumes
```

---

### Cleanup Commands

```bash
# Remove all services manually
docker service rm $(docker service ls -q)

# Leave Swarm (worker node)
docker swarm leave

# Leave Swarm (manager node - force)
docker swarm leave --force

# Prune unused resources
docker system prune -a --volumes
```

---

## 6. Service Discovery & Routing

### How Services Communicate in Docker Swarm

#### 1. **DNS-Based Service Discovery**

```yaml
# In docker-stack.yml
catalog-service:
  networks:
    - microservices-overlay

cart-service:
  networks:
    - microservices-overlay
```

**How it works**:
```bash
# Cart service calls: http://catalog-service:8080/products
# Swarm DNS resolves "catalog-service" to replica IPs
# Load balances requests across all replicas
# No hardcoded IPs needed!
```

**DNS Resolution**:
- Service name → Virtual IP (VIP)
- VIP → Round-robin to replica IPs
- Automatic updates when replicas change

---

#### 2. **API Gateway Routing in Swarm**

```
External Request          Swarm Ingress Routing Mesh
     ↓                              ↓
http://localhost:8080/catalog/products
     ↓                              ↓
┌────────────────────────────────────────┐
│    Ingress Load Balancer               │
│    (Distributes to any gateway replica)│
└────────────────────────────────────────┘
     ↓ (random selection)
┌─────────────┬─────────────┐
│  Gateway    │  Gateway    │
│ Replica 1   │ Replica 2   │
└─────────────┴─────────────┘
     ↓ (route lookup)
http://catalog-service:8080/products
     ↓ (DNS resolution)
┌─────────────┬─────────────┬─────────────┐
│  Catalog    │  Catalog    │  Catalog    │
│ Replica 1   │ Replica 2   │ Replica 3   │
└─────────────┴─────────────┴─────────────┘
```

**Flow Explanation**:

1. **External Request**: Client sends `GET /catalog/products` to `localhost:8080`

2. **Ingress Mesh**: Swarm routing mesh intercepts
   - Picks any node in cluster
   - Forwards to available gateway replica
   - Client unaware of internal routing

3. **Gateway Processing**: Spring Cloud Gateway
   - Checks routing rules (`application.yml`)
   - Route: `/catalog/**` → `lb://catalog-service`
   - Uses Eureka for service discovery
   - Sends request to `http://catalog-service:8080/**`

4. **Catalog Service**: One of 3 replicas responds
   - Swarm DNS load balances across replicas
   - Response flows back through gateway
   - Client receives result

---

#### 3. **Spring Cloud Gateway Configuration**

```yaml
# gateway-service/src/main/resources/application.yml
spring:
  cloud:
    gateway:
      routes:
        - id: catalog-route
          uri: lb://catalog-service  # Load balanced
          predicates:
            - Path=/catalog/**
          filters:
            - StripPrefix=1
```

**Key Points**:
- `lb://`: Indicates load-balanced URI
- Eureka resolves `catalog-service` to available instances
- Swarm DNS provides IPs of all replicas
- Double load balancing: Eureka + Swarm DNS

---

#### 4. **Advantages in Swarm**

| Feature | Benefit |
|---------|---------|
| **Service Names** | No IP management needed |
| **Automatic LB** | Built-in round-robin |
| **Health-Aware** | Failed replicas excluded |
| **Dynamic Updates** | New replicas auto-discovered |
| **Encryption** | Overlay network encrypts traffic |

---

## 7. Production Best Practices

### 7.1 Secrets Management

**Problem**: Storing sensitive data (passwords, API keys, JWT secrets)

#### Using Docker Secrets

```bash
# Create secret from command line
echo "my-super-secret-jwt-key" | docker secret create jwt_secret -

# Create secret from file
docker secret create database_password ./db_password.txt

# List secrets
docker secret ls
```

#### Use in Stack
```yaml
services:
  identity-service:
    secrets:
      - jwt_secret
    environment:
      - JWT_KEY_FILE=/run/secrets/jwt_secret

secrets:
  jwt_secret:
    external: true  # Created manually
```

**Benefits**:
- Encrypted at rest and in transit
- Only accessible to assigned services
- Mounted as file in `/run/secrets/`
- Not visible in `docker inspect`

---

### 7.2 Configuration Management

```yaml
configs:
  gateway_config:
    file: ./gateway-service/application.yml

services:
  gateway-service:
    configs:
      - source: gateway_config
        target: /app/application.yml
```

**Use Cases**:
- Application configuration files
- Non-sensitive data
- Version-controlled configs
- Easy updates without rebuilds

---

### 7.3 Rolling Updates Strategy

```yaml
deploy:
  replicas: 3
  update_config:
    parallelism: 1        # Update 1 at a time
    delay: 10s            # Wait 10s between updates
    failure_action: rollback  # Auto-rollback on failure
    monitor: 60s          # Monitor for 60s
    max_failure_ratio: 0.3    # Rollback if >30% fail
  restart_policy:
    condition: on-failure
    delay: 5s
    max_attempts: 3
    window: 120s
```

**Best Practices**:
- **Small Parallelism**: Update 1-2 containers at a time
- **Adequate Delay**: Allow healthchecks to stabilize
- **Auto Rollback**: Protect against bad deployments
- **Monitoring**: Watch metrics during updates

---

### 7.4 Health Checks

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/actuator/health"]
  interval: 30s
  timeout: 10s
  retries: 3
  start_period: 40s
```

**Parameters**:
- **interval**: Check frequency
- **timeout**: Max time for check
- **retries**: Failures before unhealthy
- **start_period**: Grace period on startup

**Why Important**:
- Swarm routes only to healthy replicas
- Failed containers restarted automatically
- Rolling updates wait for health

---

### 7.5 Resource Limits

```yaml
deploy:
  resources:
    limits:
      cpus: '2'
      memory: 1G
    reservations:
      cpus: '0.5'
      memory: 512M
```

**Prevents**:
- One service consuming all resources
- Memory leaks crashing host
- CPU starvation of other services

---

### 7.6 High Availability Setup

#### Multi-Manager Configuration
```bash
# On first manager
docker swarm init --advertise-addr <MANAGER1-IP>

# On second manager
docker swarm join --token <MANAGER-TOKEN> <MANAGER1-IP>:2377

# On third manager
docker swarm join --token <MANAGER-TOKEN> <MANAGER1-IP>:2377
```

**Quorum**:
- 3 managers = tolerates 1 failure
- 5 managers = tolerates 2 failures
- Always use odd numbers (3, 5, 7)

#### Placement Constraints
```yaml
# Critical services on managers
eureka-server:
  deploy:
    placement:
      constraints:
        - node.role == manager

# Spread replicas across nodes
catalog-service:
  deploy:
    placement:
      preferences:
        - spread: node.id
```

---

### 7.7 Logging Strategy

```bash
# Centralized logging with driver
docker service create \
  --log-driver json-file \
  --log-opt max-size=10m \
  --log-opt max-file=3 \
  myservice

# Stream logs
docker service logs -f microservices_gateway-service --tail 100
```

**Production Options**:
- **ELK Stack**: Elasticsearch, Logstash, Kibana
- **Splunk**: Enterprise log management
- **Fluentd**: Unified logging layer

---

### 7.8 Monitoring & Alerting

#### Prometheus & Grafana (Recommended)

```yaml
# Add to docker-stack.yml
prometheus:
  image: prom/prometheus
  volumes:
    - ./prometheus.yml:/etc/prometheus/prometheus.yml
  ports:
    - "9091:9090"
  deploy:
    placement:
      constraints:
        - node.role == manager

grafana:
  image: grafana/grafana
  ports:
    - "3001:3000"
  depends_on:
    - prometheus
```

**Metrics to Track**:
- Request rate (RPS)
- Error rate (4xx/5xx)
- Response time (p50, p95, p99)
- CPU/Memory usage per service
- Container restart count

---

### 7.9 Backup Strategy

```bash
# Backup Swarm state
docker swarm ca > swarm-ca-backup.pem

# Backup volumes (if using)
docker run --rm -v myvolume:/data -v $(pwd):/backup \
  alpine tar czf /backup/volume-backup.tar.gz /data
```

---

### 7.10 Security Checklist

- ✅ Use Docker secrets for credentials
- ✅ Enable overlay network encryption
- ✅ Run containers as non-root user
- ✅ Scan images for vulnerabilities (`docker scan`)
- ✅ Use private registry for images
- ✅ Limit exposed ports (only gateway)
- ✅ Enable Docker Content Trust (DCT)
- ✅ Regular security updates
- ✅ Implement network policies
- ✅ Use firewall rules between nodes

---

## 8. Troubleshooting

### Common Issues & Solutions

#### Issue 1: Service won't start
```bash
# Check service status
docker service ps microservices_catalog-service --no-trunc

# View detailed logs
docker service logs microservices_catalog-service --tail 50

# Check node resources
docker node ls
docker node inspect <node-id>
```

**Common Causes**:
- Image pull failure
- Port conflicts
- Resource constraints
- Health check failures

---

#### Issue 2: Services can't communicate
```bash
# Verify network
docker network ls
docker network inspect microservices_microservices-overlay

# Test DNS resolution
docker run --rm --network microservices_microservices-overlay \
  alpine ping catalog-service
```

**Solutions**:
- Ensure all services on same network
- Check firewall rules
- Verify overlay network is attachable

---

#### Issue 3: Load balancing not working
```bash
# Check replica count
docker service ls

# View where replicas are running
docker service ps microservices_gateway-service

# Test load distribution
for i in {1..10}; do curl localhost:8080/catalog/products; done
```

---

#### Issue 4: Updates failing
```bash
# Check update status
docker service inspect microservices_catalog-service --pretty

# View update history
docker service ps microservices_catalog-service

# Manual rollback
docker service rollback microservices_catalog-service
```

---

## Appendix: Quick Reference

### Essential Commands Cheat Sheet

```bash
# ===== SWARM MANAGEMENT =====
docker swarm init                    # Initialize swarm
docker swarm join-token worker       # Get worker join token
docker swarm leave --force           # Leave swarm (manager)

# ===== STACK OPERATIONS =====
docker stack deploy -c stack.yml app # Deploy stack
docker stack ls                      # List stacks
docker stack services app            # List services in stack
docker stack rm app                  # Remove stack

# ===== SERVICE MANAGEMENT =====
docker service ls                    # List services
docker service ps <service>          # List tasks
docker service logs <service> -f     # Stream logs
docker service scale <service>=N     # Scale service
docker service update <service>      # Update service
docker service rollback <service>    # Rollback service

# ===== NODE MANAGEMENT =====
docker node ls                       # List nodes
docker node inspect <node>           # Node details
docker node update --availability drain <node>  # Drain node

# ===== SECRETS & CONFIGS =====
docker secret create <name> <file>   # Create secret
docker secret ls                     # List secrets
docker config create <name> <file>   # Create config
docker config ls                     # List configs
```

---

## Conclusion

You've successfully migrated from Docker Compose to Docker Swarm! 🎉

**Next Steps**:
1. Deploy stack: `docker stack deploy -c docker-stack.yml microservices`
2. Access visualizer: `http://localhost:9090`
3. Monitor services: `docker service ls`
4. Test scaling: `docker service scale microservices_catalog-service=5`
5. Implement secrets management
6. Set up monitoring with Prometheus/Grafana
7. Configure CI/CD for automated deployments

**Resources**:
- [Docker Swarm Documentation](https://docs.docker.com/engine/swarm/)
- [Spring Cloud Gateway](https://spring.io/projects/spring-cloud-gateway)
- [Docker Secrets](https://docs.docker.com/engine/swarm/secrets/)
- [Swarm Best Practices](https://docs.docker.com/engine/swarm/swarm-tutorial/)

---

**Author**: Bouyahi's DevOps Team  
**Version**: 1.0  
**Last Updated**: 2026-01-04
