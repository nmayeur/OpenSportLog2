# Utilisez une image de base adapt�e � votre application
FROM node:18

# D�finissez le r�pertoire de travail dans le conteneur
WORKDIR /app

# Copiez les fichiers n�cessaires dans le conteneur
COPY package*.json ./
COPY . .

# Installez les d�pendances
RUN npm install

# Exposez le port sur lequel l'application �coute
EXPOSE 3000

# Commande pour d�marrer l'application
CMD [ "npm", "run", "dev" ]
