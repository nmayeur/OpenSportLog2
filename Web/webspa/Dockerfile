# Utilisez une image de base adaptée à votre application
FROM node:18

# Définissez le répertoire de travail dans le conteneur
WORKDIR /app

# Copiez les fichiers nécessaires dans le conteneur
COPY package*.json ./
COPY . .

# Installez les dépendances
RUN npm install

# Exposez le port sur lequel l'application écoute
EXPOSE 3000

# Commande pour démarrer l'application
CMD [ "npm", "run", "dev" ]
