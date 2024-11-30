data "aws_db_instance" "database" {
  db_instance_identifier = var.rds_cluster_name
}

locals {
  connection_string = "Server=${trimsuffix(data.aws_db_instance.database.endpoint, ":${data.aws_db_instance.database.port}")};Port=${data.aws_db_instance.database.port};Database=${data.aws_db_instance.database.db_name};Uid=${var.db_user};Pwd=${var.db_password};"
}

resource "kubernetes_secret" "catalog_secret" {
  metadata {
    name = "catalog_secret"
  }

  data = {
    "ConnectionStrings__Default" = local.connection_string
  }
}

resource "kubernetes_deployment" "catalog_deployment" {
  depends_on = [ kubernetes_secret.catalog_secret ]
  metadata {
    name = "catalog_deployment_tf"
    labels = {
      nome = "catalog"
    }
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        nome = "catalog"
      }
    }

    template {
      metadata {
        labels = {
          nome = "catalog"
        }
      }

      spec {
        container {
          name  = "catalog"
          image = var.ecr_repository_name

          port {
            container_port = 80
          }

          env_from {
            secret_ref {
              name = "catalog_secret"
            }
          }

          resources {
            requests = {
              cpu    = "100m"
              memory = "120Mi"
            }
            limits = {
              cpu    = "150m"
              memory = "200Mi"
            }
          }

          liveness_probe {
            http_get {
              port = 80
              path = "/api/produtos/categoria/0"
            }
            period_seconds        = 30
            failure_threshold     = 5
            initial_delay_seconds = 40
            timeout_seconds = 15
          }

          readiness_probe {
            http_get {
              port = 80
              path = "/api/produtos/categoria/0"
            }
            period_seconds        = 30
            failure_threshold     = 5
            initial_delay_seconds = 40
            timeout_seconds = 15
          }
        }
      }
    }
  }
}

resource "kubernetes_horizontal_pod_autoscaler_v2" "catalog_hpa" {
  metadata {
    name = "catalog_hpa"
  }

  spec {
    scale_target_ref {
      kind        = "Deployment"
      name        = "catalog_deployment"
      api_version = "apps/v1"
    }

    min_replicas = 1
    max_replicas = 2

    metric {
      type = "ContainerResource"
      container_resource {
        container = "api"
        name      = "cpu"
        target {
          average_utilization = 65
          type = "Utilization"
        }
      }
    }
  }
}

resource "kubernetes_service" "svc_catalog_loadbalancer" {
  metadata {
    name = "svc-catalog-loadbalancer"
    annotations = {
      "service.beta.kubernetes.io/aws-load-balancer-type": "nlb"
      "service.beta.kubernetes.io/aws-load-balancer-scheme": "internal"
      "service.beta.kubernetes.io/aws-load-balancer-cross-zone-load-balancing-enabled": "true"
    }
  }

  spec {
    port {
      port        = 80
      target_port = 80
      node_port   = 30007
    }

    selector = {
      app = "catalog"
    }

    type = "LoadBalancer"
  }
}