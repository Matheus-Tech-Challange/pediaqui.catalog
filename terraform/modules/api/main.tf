data "aws_db_instance" "database" {
  db_instance_identifier = var.rds_cluster_name
}

locals {
  connection_string = "Server=${trimsuffix(data.aws_db_instance.database.endpoint, ":${data.aws_db_instance.database.port}")};Port=${data.aws_db_instance.database.port};Database=${data.aws_db_instance.database.db_name};Uid=${var.db_user};Pwd=${var.db_password};"
}

resource "kubernetes_secret" "catalog_secret" {
  metadata {
    name = "catalog-secret"
  }

  data = {
    "ConnectionStrings__Default" = local.connection_string
  }
}

resource "kubernetes_deployment" "catalog_deployment" {
  depends_on = [ kubernetes_secret.catalog_secret ]
  metadata {
    name = "catalog-deployment-tf"
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
              name = "catalog-secret"
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
        }
      }
    }
  }
}

resource "kubernetes_service" "svc_catalog_loadbalancer" {
  metadata {
    name = "svc-catalog-loadbalancer"
    annotations = {
      "service.beta.kubernetes.io/aws-load-balancer-type": "nlb" # ou "clb" para Classic
      "service.beta.kubernetes.io/aws-load-balancer-scheme": "internet-facing" # público
      "service.beta.kubernetes.io/aws-load-balancer-cross-zone-load-balancing-enabled": "true"
    }
  }

  spec {
    port {
      port        = 80
      target_port = 80
      #node_port   = 30007
    }

    selector = {
      app = "catalog"
    }

    type = "LoadBalancer"
  }
}