# Étape 1 : choisir une image de base avec JDK 17
FROM eclipse-temurin:17-jdk-jammy

# Étape 2 : définir le répertoire de travail
WORKDIR /app

# Étape 3 : copier le jar dans le conteneur
COPY target/firstMS-0.0.1-SNAPSHOT.jar app.jar

# Étape 4 : exposer le port défini dans application.properties
EXPOSE 8081

# Étape 5 : commande pour exécuter l'application
ENTRYPOINT ["java", "-jar", "app.jar"]